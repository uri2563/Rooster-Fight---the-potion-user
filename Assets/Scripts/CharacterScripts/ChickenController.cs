using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CapsuleCollider))]
public class ChickenController : MonoBehaviour
{
    private const float MIN_RUSH_DISTANCE = 4f;
    private const float RUSH_SPEED = 6f;
    private const float RUSH_POWER = 7f;
    private const float RUSH_TIME = 1f;

    private const float MIN_KICK_DISTANCE = 1.2f;
    private const float KICK_POWER = 5f;
    private const float KICK_TIME = 1.1f;

    private const float MIN_PECK_DISTANCE = 1f;
    private const float PECK_POWER = 4f;
    private const float PECK_TIME = 1f;

    private const int MAX_ATTACK_IN_ROW = 3;

    private const float ROTATION_SPEED = 10f;
    public enum Actions { RUSH, KICK, PECK, MOVE , DEAD }
    Actions currentAction;
    public int getAction() { return (int)currentAction; }

    enum State {ACTION , NONE}
    State currentState;

    private AudioManager audioManager;

    public Stats stats;
    private NavMeshAgent myNav;

    public List<string> currentBuffs;
    public List<PotionBuff> buffs;

    [SerializeField]
    private GameObject opponent = null;
    private ChickenController opponentController;

    public GameObject floatingTextPrefab;

    //5 input - myloc-x,z ,oploc-x,z, distance
    //6 output - action - rush, kick, peck, walk, target-x,z
    private NeuralNet net;

    //Debug
    public Vector3 destaniation;
    public int num_chicken;
    private void Start()
    {
        audioManager = AudioManager.Instance;

        //debug:
        num_chicken = Random.Range(0, 1000);

        currentState = State.NONE;
        currentAction = Actions.MOVE;

        currentBuffs = new List<string>();

        if (!GetComponent<CapsuleCollider>().isTrigger)
        { Debug.LogError("No colider trigger for this char"); }

        myNav = GetComponent<NavMeshAgent>();
        if (myNav == null)
        {
            Debug.LogError(num_chicken + "No nav mesh agent on Character: " + gameObject.name);
        }
    }

    private void FindOpponent()
    {
        opponent = findTargetPosition();
        opponentController = opponent.GetComponent<ChickenController>();
    }

    public void setNet(NeuralNet net)
    {
        this.net = net;
    }
    public NeuralNet getNet()
    {
        return net;
    }

    private List<float> output;
    
    private void FixedUpdate()
    {
        if(stats != null && stats.CheckDead())
        {
            Debug.Log("DEAD");
            myNav.isStopped = true;
            currentAction = Actions.DEAD;
            currentState = State.ACTION;
        }
        else if (currentState.Equals(State.NONE) && (opponentController == null || !opponentController.stats.CheckDead()))
        {
            currentState = State.ACTION;
            AI();
        }
    }

    private void AI()
    {
        if (opponent == null)
        {
            FindOpponent();
        }

        List<float> input;

        input = new List<float>();
        input.Add(opponent.transform.position.x);
        input.Add(opponent.transform.position.z);
        input.Add(transform.position.x);
        input.Add(transform.position.z);
        input.Add(Vector3.Distance(opponent.transform.position,transform.position)/2f);


        output = net.Input(input);
        int action = FindMaxIndex(output, 3);

       // Debug.Log(num_chicken+" output: " +output[0] + "," + output[1] + "," + output[2]);

        Debug.Log(num_chicken + " Attack:" + ((Actions)action).ToString());

        MakeAttack(action);
    }

    private void MakeAttack(int action)
    {
        switch (action)
        {
            case (int)Actions.RUSH:
                StartCoroutine(Attack(MIN_RUSH_DISTANCE, RUSH_POWER * stats.getPower(), RUSH_TIME, Actions.RUSH));
                break;
            case (int)Actions.KICK:
                StartCoroutine(Attack(MIN_KICK_DISTANCE, KICK_POWER * stats.getPower(), KICK_TIME, Actions.KICK));
                break;
            case (int)Actions.PECK:
                StartCoroutine(Attack(MIN_PECK_DISTANCE, PECK_POWER * stats.getPower(), PECK_TIME, Actions.PECK));
                break;
            default: // case it is 4
                Vector3 location = new Vector3(output[3] * 5, transform.position.y, output[4] * 5);
                StartCoroutine(MoveToLoctaion(location, stats.getSpeed()));
                break;
        }
    }

    IEnumerator Attack(float min, float damage, float attckTime, Actions action)
    {
        float speed = stats.getSpeed();
        if (action.Equals(Actions.RUSH))
        {
            speed = speed * RUSH_SPEED;
        }
        if (CoolAttack(action))
        {
            yield return new WaitForSeconds(1f);
        }

        else
        {
            //get far
            float timer = 0;
            while (GetDistanceToOpponent() < min && timer < 1f)
            {
               // Debug.Log(num_chicken + " getting away: " + action.ToString());
                currentAction = Actions.MOVE;
                timer += Time.deltaTime;
                Vector3 away = (transform.position - opponent.transform.position);
                SetDestenation(away.normalized * 3f, stats.getSpeed()*2);
                yield return null;
            }

            while(Vector3.Dot((opponent.transform.position - transform.position).normalized, transform.forward) < 0.7f && FaceTarget())//look at target
            {
                yield return null;
            }

            //attack only if you got to min attack distance
            if (GetDistanceToOpponent() > min)
            {
                if (action.Equals(Actions.RUSH))
                {
                    currentAction = action;
                }
                else
                {
                    currentAction = Actions.MOVE;
                }

                timer = 0;
                while (Vector3.Distance(opponent.transform.position, transform.position) > myNav.stoppingDistance && timer < 5f)
                {
                    SetDestenation(opponent.transform.position, speed);
                    timer += Time.deltaTime;
                    yield return null;
                }
                if(Vector3.Distance(opponent.transform.position, transform.position) < myNav.stoppingDistance + 0.1f)//check that got to the enemy
                {
                    //print(num_chicken + "reached opponent");
                    currentAction = action;
                    timer = 0;
                    while (timer < attckTime)
                    {
                        FaceTarget();
                        timer += Time.deltaTime;
                        yield return null;
                    }
                    audioManager.Play(action.ToString());
                    MakeDamage(damage);
                    myNav.velocity = Vector3.zero;//stop in place
                }
            }
            currentAction = Actions.MOVE;
            currentState = State.NONE;
        }

    }

    private int attack_counter = 0;
    private Actions lastAttack = Actions.MOVE;
    private bool CoolAttack(Actions action)
    {
        if(lastAttack.Equals(action))
        {
            attack_counter++;
            if(attack_counter > MAX_ATTACK_IN_ROW)
            {
                //Debug.Log("unstuck");
                //attack_counter = MAX_ATTACK_IN_ROW - 1;//allow doing only one attack but pay in cooldown
                if(attack_counter >= MAX_ATTACK_IN_ROW*2)//make random attack is stuck too long
                {
                    MakeAttack(Random.Range(0,2));
                }
                else//walk
                {
                    StartCoroutine(MoveToLoctaion(new Vector3(output[3] * 5f, transform.position.y, output[4] * 5f), stats.getSpeed()));
                }
                return true;
            }
        }
        else
        {
            lastAttack = action;
            attack_counter = 0;
        }
        return false;
    }

    //moves
    IEnumerator MoveToLoctaion(Vector3 location, float speed)
    {
        if (CalculateNewPath(location))
        {
            audioManager.Play("WalkAround");
            currentAction = Actions.MOVE;
            SetDestenation(location, speed);
            float timer = 0;
            while (!NavReachedDestination() && timer < 3f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            //not real location
            if(timer <= 0.1f)
            {
                //set random destination
                Vector3 random_location = new Vector3(Random.Range(-5f, 5f), transform.position.z,Random.Range(-5f, 5f));
                SetDestenation(random_location, speed);
                //Debug.Log("movestuck: " + random_location.x + "," + random_location.z);
                while (!NavReachedDestination() && timer < 3f)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }
            }
        }
        else
        {
            Debug.Log("NOT to location: " + location.x + "," + location.z);
        }

        currentState = State.NONE;
    }

    private GameObject findTargetPosition()
    {
        GameObject[] chickens = GameObject.FindGameObjectsWithTag("Chicken");
        for (int i = 0; i < chickens.Length; i++)
        {
            if (chickens[i] != this.gameObject)
            if (chickens[i] != this.gameObject)
            {
                return chickens[i];
            }
        }
       // Debug.LogError(num_chicken + "opponent doesnt exists");
        return null;
    }

    private static int FindMaxIndex(List<float> output, int size)
    {
        float max = output[0];
        int index = 0;
        for (int i = 1; i < size; i++)
        {
            if (output[i] > max)
            {
                max = output[i];
                index = i;
            }
        }
        return index;
    }

    private bool NavReachedDestination()
    {
        if (myNav == null || !myNav.isActiveAndEnabled)
        {
            return true;
        }

        //Debug.Log(num_chicken + "NavReachedDestination");
        if (!myNav.pathPending)
        {
            if (myNav.remainingDistance <= myNav.stoppingDistance)
            {

                if (!myNav.hasPath || myNav.velocity.sqrMagnitude == 0f)
                {
                    return true;// Done
                }
            }
        }
        return false;
    }
    private bool CalculateNewPath(Vector3 location)
    {
        if (myNav == null)
        {
            Debug.LogWarning("no navmesh");
            return false;
        }

        NavMeshPath navMeshPath = new NavMeshPath();
        myNav.CalculatePath(location, navMeshPath);
        if (navMeshPath.status != NavMeshPathStatus.PathComplete)
        {
            Debug.Log("cant move to wanted location");
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool FaceTarget()
    {
        Transform target = opponent.transform;
        Vector3 direction = (target.position - transform.position).normalized;

        if(direction.magnitude == 0)
        {
            Debug.Log("zero");
            return false;
        }

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * ROTATION_SPEED);
        return true;
    }

    private void MakeDamage(float damage)
    {
        //Debug.Log(num_chicken + "damage opp: " + damage);
        Stats s_damage = new Stats((int)damage,0 , 0 ,0);
        opponentController.stats.ReduceStats(s_damage);

        //show damage
        GameObject fText = Instantiate(floatingTextPrefab, opponent.transform.position, Quaternion.identity, opponent.transform);
        fText.GetComponent<TextMesh>().text = ((int)damage).ToString();
        audioManager.Play("GetHit");
    }

    private float GetDistanceToOpponent()
    {
        return Vector3.Distance(opponent.transform.position, this.transform.position);
    }

    private void SetDestenation(Vector3 destantation, float speed)
    {
        if (myNav == null || !myNav.isActiveAndEnabled)
        {
            return;
        }

        this.destaniation = destantation;

        myNav.speed = speed;
        myNav.acceleration = speed;
        myNav.SetDestination(destantation);
    }

    //****************************************************************************************//
    //potions
    public ParticleSystem particalHitPrefab;
    private ScriptalbePotion potionHit;
    private void OnTriggerEnter(Collider other)
    {
        //if got hit by potion
        if (other.gameObject.tag == "Potion")
        {
            potionHit = other.gameObject.GetComponent<PotionController>().GetScriptablePotion();

            Debug.Log("potion hit");
            AudioManager.Instance.Play("Splash");

            ParticleSystem prefabParticals = Instantiate(particalHitPrefab, transform.position, Quaternion.identity);
            Destroy(prefabParticals.gameObject, 1f);

            if(stats.CheckDead())
            {
                return;
            }

            foreach (PotionBuff pot in buffs)
            {
                if(pot.buffName.Equals(potionHit.Name))
                {
                    pot.ResetPotionEffect();
                    return;
                }
            }

            foreach (PotionBuff pot in buffs)
            {
                if (pot.buffName.Equals(""))
                {
                    pot.StartBuff(potionHit);
                    return;
                }
            }
        }
    }
}