
using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private const float throwPower = 500;

    public ScriptalbePotion scriptalbePotion;
    public PotionSlot potionSlot;

    public bool SetPotionView(ScriptalbePotion _scriptalbePotion)
    {
        scriptalbePotion = _scriptalbePotion;

        if(_scriptalbePotion == null)
        {
            this.gameObject.SetActive(false);
            return true;
        }
        
        //set mesh
        this.GetComponent<MeshFilter>().sharedMesh = scriptalbePotion.meshFilter.sharedMesh;
        this.GetComponent<MeshRenderer>().sharedMaterials = scriptalbePotion.material;

        return true;
    }

    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeFinish, timeInterval;
    bool canTouch = true;

    public void SetTouch(bool state)
    { canTouch = state; }

    [SerializeField]
    float throwForceInXandY = 1f;

    [SerializeField]
    float throwForceInZ = 50f;

    GameObject potionGeneralPrefab;
    int cooldownTime;//get from prefab/scriptable

    private void Start()
    {
        potionGeneralPrefab = Resources.Load<GameObject>("Prefabs/PotionPrefab");
    }

    private void OnMouseDown()
    {
        if (canTouch)
        {
            //get time of start swipe and position
            touchTimeStart = Time.time;
            startPos = Input.mousePosition; // can be replaced with Input.GetTouch(0).position - for 
        }
    }
    public GameObject center;
    public Camera cam;
    private void OnMouseUp()
    {
        if (canTouch)
        {
            canTouch = false;
            potionSlot.StartCoolDown(scriptalbePotion.coolDownTime);

            //try 1:
            /*
            touchTimeFinish = Time.time;

            //marks time interval start to finish
            timeInterval = touchTimeFinish - touchTimeStart;

            //get finish pos
            endPos = Input.mousePosition; // can be replaced with Input.GetTouch(0).position - for app

            //swipe diraction
            direction = startPos - endPos;
            
            //set throw dirction vector
            Vector3 throwDircation = new Vector3(-direction.x * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);
            
            //make the prefab
            GameObject potionPrefab = Instantiate(potionGeneralPrefab, transform.position, Quaternion.identity);
            SetUpPrefab(potionPrefab);

            Debug.DrawRay(transform.position, (throwDircation.normalized * throwPower),Color.blue,10f);//debug

            ThrowPotion(potionPrefab, transform.position, (throwDircation.normalized * throwPower));
            */

            /*
            //try 2: check if works in phone (can try in update) - https://stackoverflow.com/questions/44177968/throwing-object-to-a-specific-point-in-space-using-touch
            Touch touch = Input.GetTouch(0);
            Vector3 touchPos = new Vector3(touch.position.x, touch.position.y, 0);
            Debug.Log("You are touching at position: " + touchPos);
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hitResult;
            Physics.Raycast(ray, out hitResult);
            Vector3 throwDestination = hitResult.point;
            Debug.Log("Throw destination is " + throwDestination);

            //make the prefab
            GameObject potionPrefab = Instantiate(potionGeneralPrefab, transform.position, Quaternion.identity);
            SetUpPrefab(potionPrefab);

            ThrowPotion(potionPrefab, transform.position, throwDestination);
            */

            //try 3:
            /*
            //current time
            touchTimeFinish = Time.time;
            //marks time interval start to finish
            timeInterval = touchTimeFinish - touchTimeStart;

            //offset
            float x = (cam.WorldToScreenPoint(transform.position).x - cam.WorldToScreenPoint(center.transform.position).x)/10f;

            //get finish pos
            endPos = Input.mousePosition; // can be replaced with Input.GetTouch(0).position - for app
            //swipe diraction
            direction = startPos - endPos;

            //set throw dirction vector
            Vector3 throwDircation = new Vector3((x-direction.x) /2f, -direction.y / 2f, -direction.y / 2f);

            //make the prefab
            GameObject potionPrefab = Instantiate(potionGeneralPrefab, transform.position, Quaternion.identity);
            SetUpPrefab(potionPrefab);
            //visual debug
            Debug.DrawRay(transform.position, (throwDircation.normalized * throwPower),Color.blue,10f);//debug
            //throw

            ThrowPotion(potionPrefab, transform.position, (throwDircation * 2f));
            */

            //try 4:
            
            touchTimeFinish = Time.time;

            //marks time interval start to finish
            timeInterval = touchTimeFinish - touchTimeStart;

            //get finish pos
            endPos = Input.mousePosition; // can be replaced with Input.GetTouch(0).position - for app

            //swipe diraction
            direction = startPos - endPos;

            //offset
            float x = (cam.WorldToScreenPoint(transform.position).x - cam.WorldToScreenPoint(center.transform.position).x) / 10f;
            
            //set throw dirction vector
            Vector3 throwDircation = new Vector3((x-direction.x) * throwForceInXandY, -direction.y * throwForceInXandY, throwForceInZ / timeInterval);
            
            //make the prefab
            GameObject potionPrefab = Instantiate(potionGeneralPrefab, transform.position, Quaternion.identity);
            SetUpPrefab(potionPrefab);

            Debug.DrawRay(transform.position, (throwDircation.normalized * throwPower),Color.blue,10f);//debug
            Debug.Log("throw d: " + throwDircation.ToString() + " throw nor: " + throwDircation.normalized.ToString() + " time: "+timeInterval);

            ThrowPotion(potionPrefab, transform.position, (throwDircation.normalized * throwPower));
            

            //try 5:
            //get finish pos
            /*
            endPos = Input.mousePosition; // can be replaced with Input.GetTouch(0).position - for app

            //swipe diraction
            direction = startPos - endPos;

            //offset
            float x = (cam.WorldToScreenPoint(transform.position).x - cam.WorldToScreenPoint(center.transform.position).x) / 10f;

            //set throw dirction vector
            Vector3 throwDircation = new Vector3((x-direction.x), 40f, -direction.y);
            Debug.Log("throw: " + throwDircation.ToString());
            //make the prefab
            GameObject potionPrefab = Instantiate(potionGeneralPrefab, transform.position, Quaternion.identity);
            SetUpPrefab(potionPrefab);

            Debug.DrawRay(transform.position, (throwDircation),Color.blue,10f);//debug

            ThrowPotion(potionPrefab, transform.position, throwDircation.normalized * throwPower);
            */

            //try 6:
            /*
            touchTimeFinish = Time.time;

            //marks time interval start to finish
            timeInterval = touchTimeFinish - touchTimeStart;

            //get finish pos
            endPos = Input.mousePosition; // can be replaced with Input.GetTouch(0).position - for app

            //swipe diraction
            direction = startPos - endPos;

            //offset
            float x = transform.position.x - center.transform.position.x;

            //set throw dirction vector
            Vector3 throwDircation = new Vector3((x - direction.x) * throwForceInXandY, timeInterval * 100f, -direction.y * throwForceInXandY);

            //make the prefab
            GameObject potionPrefab = Instantiate(potionGeneralPrefab, transform.position, Quaternion.identity);
            SetUpPrefab(potionPrefab);

            Debug.DrawRay(transform.position, (throwDircation.normalized * throwPower), Color.blue, 10f);//debug
            Debug.Log("throw d: " + throwDircation.ToString() + " throw nor: " + throwDircation.normalized.ToString());

            ThrowPotion(potionPrefab, transform.position, (throwDircation.normalized * throwPower));
            */
        }
    }
    private void SetUpPrefab(GameObject prefab)
    {
        prefab.GetComponent<PotionController>().SetPotionPrefab(scriptalbePotion);
    }


    private static void ThrowPotion(GameObject potionPrefab, Vector3 startposition, Vector3 throwDircation)
    {
        Rigidbody rb = potionPrefab.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(throwDircation);
        //rb.velocity = throwDircation;
    }
}
