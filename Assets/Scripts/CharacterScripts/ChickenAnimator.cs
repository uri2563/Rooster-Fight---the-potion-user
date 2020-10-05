using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChickenAnimator : MonoBehaviour
{
    const float locoMotionAnimationSmoothTime = 0.1f;

    private Animator animator;
    private NavMeshAgent agent;
    private ChickenController controllerChicken;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        controllerChicken = GetComponent<ChickenController>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speed", speed, locoMotionAnimationSmoothTime, Time.deltaTime);
        animator.SetInteger("attack", controllerChicken.getAction());
    }
}
