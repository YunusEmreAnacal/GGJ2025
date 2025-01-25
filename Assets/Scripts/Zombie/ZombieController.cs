using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    private NavMeshAgent agent = null;
    private Animator animator = null;
    private Transform target;


    private bool inTrigger = false;

    [SerializeField] private float wanderRadius = 10f;

    private Vector3 wayPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        AssignWaypoint();

    }

    void Update()
    {
        if (inTrigger)
        {
            MoveToTarget();
        }
        else
        {
            float distanceToTarget = Vector3.Distance(transform.position, wayPoint);
            if (distanceToTarget < agent.stoppingDistance * 1.1f)
            {
                     AssignWaypoint();
            }
            if (agent.destination != wayPoint){
                agent.SetDestination(wayPoint);
            }
            
            animator.SetBool("isWalking", agent.speed > 0.5f); // Walk animation if moving

        }


    }


    private void AssignWaypoint(){
        wayPoint = RandomNavSphere(transform.position, wanderRadius, -1);
    }

    private Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask); //harita dışı
        return navHit.position;
    }


    private void MoveToTarget()
    {
        agent.SetDestination(target.position);

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        animator.SetBool("isAttacking", distanceToTarget <= (animator.GetBool("isAttacking") ? agent.stoppingDistance * 1.5f : agent.stoppingDistance));
        agent.isStopped = animator.GetBool("isAttacking");
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        if (animator.GetBool("isAttacking"))
        {
            RotateToTarget();
        }
    }



    private void RotateToTarget()
    {
        Vector3 direction = (target.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.transform;
            inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;

        }
    }
}
