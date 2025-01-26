using UnityEngine;

public class SkeletonAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackRadius = 1.5f;        // How wide the attack circle is
    [SerializeField] private float attackRange = 2f;           // How far the attack reaches
    [SerializeField] private float attackCooldown = 2f;        // Time between attacks
    [SerializeField] private int attackDamage = 10;            // Damage per attack
    [SerializeField] private LayerMask targetLayer;            // What layers we can hit
    
    [Header("Attack Position")]
    [SerializeField] private Vector3 attackOffset = new Vector3(0, 1f, 0);  // Height of the attack circle
    
    [Header("Debug Visualization")]
    [SerializeField] private bool showAttackRange = true;      // Toggle for debug visualization
    [SerializeField] private Color gizmoColorIdle = Color.yellow;
    [SerializeField] private Color gizmoColorAttacking = Color.red;

    // Component references
    private Animator animator;
    private Transform target;             // Reference to player or target
    
    // Attack state tracking
    private float nextAttackTime;         // When we can attack next
    private bool isAttacking;             // Are we currently in attack animation
    private const string ATTACK_TRIGGER = "Attack";  // Animation trigger name

    void Start()
    {
        // Get required components
        animator = GetComponent<Animator>();
        
        // Find player if we don't have a target
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    void Update()
    {
        if (target == null) return;

        // Check if we can attack
        if (CanAttack())
        {
            // Check if target is in range
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                StartAttack();
            }
        }
    }

    private bool CanAttack()
    {
        // Check if enough time has passed since last attack
        return Time.time >= nextAttackTime && !isAttacking;
    }

    private void StartAttack()
    {
        // Start attack sequence
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;
        
        // Trigger attack animation
        if (animator != null)
        {
            animator.SetTrigger(ATTACK_TRIGGER);
        }
    }

    // Called via animation event at the attack point of the animation
    public void OnAttackPoint()
    {
        // Perform the actual circular attack
        PerformCircleAttack();
    }

    // Called via animation event at the end of attack animation
    public void OnAttackEnd()
    {
        isAttacking = false;
    }

    private void PerformCircleAttack()
    {
        // Calculate attack position (in front of the skeleton)
        Vector3 attackCenter = transform.position + transform.forward * (attackRange / 2f) + attackOffset;

        // Perform overlap sphere check to find targets
        Collider[] hitColliders = Physics.OverlapSphere(attackCenter, attackRadius, targetLayer);
        Debug.Log("Performing attack");
        // Deal damage to all hit targets
        foreach (Collider hitCollider in hitColliders)
        {
            Debug.Log(hitCollider.gameObject.name);
            // Check if hit object has a health component
            Character healthComponent = hitCollider.GetComponent<Character>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(attackDamage, transform.position);
            }
        }
    }

    // Visualization for attack range and area
    void OnDrawGizmosSelected()
    {
        if (!showAttackRange) return;

        // Draw attack range circle
        Gizmos.color = isAttacking ? gizmoColorAttacking : gizmoColorIdle;
        
        // Calculate the attack position
        Vector3 attackCenter = transform.position + transform.forward * (attackRange / 2f) + attackOffset;
        
        // Draw attack sphere
        Gizmos.DrawWireSphere(attackCenter, attackRadius);
        
        // Draw line to show attack range
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * attackRange);
    }
}