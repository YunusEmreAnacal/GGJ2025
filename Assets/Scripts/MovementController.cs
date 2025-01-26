using System;
using UnityEngine;
using UnityEngine.AI;

public class MovementController : MonoBehaviour
{
        // References to required components
    private NavMeshAgent m_Agent;
    private Animator m_Animator;

    // Configuration parameters
    [SerializeField] private float minDistanceToTarget = 1.5f;  // Minimum distance to maintain from target
    [SerializeField] private float baseMovementSpeed = 3.5f;    // Base movement speed
    [SerializeField] private float rotationSpeed = 5f;          // How fast the enemy rotates to face target

    // Animation parameter names - stored as constants to avoid typos
    private const string RUNNING_PARAM = "Running";

    private Transform target;

    void Start()
    {
        // Get required components
        m_Animator = GetComponent<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();

        // Initialize NavMeshAgent properties
        if (m_Agent != null)
        {
            m_Agent.stoppingDistance = minDistanceToTarget;
            m_Agent.speed = baseMovementSpeed;
            m_Agent.angularSpeed = rotationSpeed * 100f; // Convert to degrees
        }
        else
        {
            Debug.LogError("NavMeshAgent component missing from " + gameObject.name);
        }

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        // Update destination and handle movement
        UpdateMovement();
        
        // Update animations
        UpdateAnimations();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    private void UpdateMovement()
    {
        if (target == null) return;
        
        // Set new destination for the agent
        m_Agent.destination = target.transform.position;
        // Smoothly rotate to face the target
        Vector3 directionToTarget = (target.transform.position - transform.position).normalized;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void UpdateAnimations()
    {
        if (m_Animator != null)
        {
            // Check if the enemy is moving (using a small threshold to account for floating-point imprecision)
            bool isMoving = m_Agent.velocity.magnitude > 0.1f;
            m_Animator.SetBool(RUNNING_PARAM, isMoving);
        }
    }

    // void OnAnimatorMove()
    // {
    //     // Sync movement speed with animation
    //     if (m_Animator != null && m_Animator.GetBool(RUNNING_PARAM))
    //     {
    //         float animationSpeed = (m_Animator.deltaPosition / Time.deltaTime).magnitude;
    //         m_Agent.speed = Mathf.Clamp(animationSpeed, 0f, baseMovementSpeed * 1.5f);
    //     }
    // }

    // Optional: Add visualization for debugging
    void OnDrawGizmosSelected()
    {
        // Draw the stopping distance radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceToTarget);
    }
}
