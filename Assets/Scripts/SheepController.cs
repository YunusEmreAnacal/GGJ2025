using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SheepController : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    private NavMeshAgent agent;
    private float timer; 

    private Animator animator;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        animator = GetComponent<Animator>();
    }

    void Update()
    {      //waypoint 
        
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }

            animator.SetBool("walk", agent.velocity.magnitude > 0.1f);
        
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
}
