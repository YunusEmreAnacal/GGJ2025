using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPC_Data : MonoBehaviour
{
    public float maxHealth = 100f;
    protected float currentHealth;
    protected Animator animator;
    protected NavMeshAgent agent;
    public Rigidbody rb;

    [SerializeField] private float dieAnimationDuration;

    public Renderer npcRenderer;
    public Color hitColor = Color.red;
    public float colorChangeDuration = 0.2f;
    protected Color originalColor;

    protected Vector3 lastAttackerPosition;
    protected bool isDead = false;

    public delegate void DeathEventHandler(GameObject deadNPC);
    public event DeathEventHandler OnDeathEvent;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        originalColor = npcRenderer.material.color;
    }

    public virtual void TakeDamage(float damage, Vector3 attackerPosition)
    {
        if (isDead) return;

        currentHealth -= damage; //
        lastAttackerPosition = attackerPosition;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(ChangeColor());
        }
    }

    protected virtual void Die()
    {
        if (isDead) return;

        isDead = true;
        animator.SetTrigger("die"); // ragdoll 
        agent.isStopped = true;
        agent.enabled = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;

        StartCoroutine(DieRoutine());
    }

    protected virtual IEnumerator DieRoutine()
    {
        npcRenderer.material.color = hitColor;
        yield return new WaitForSeconds(dieAnimationDuration);

        OnDeathEvent?.Invoke(gameObject);
        Destroy(gameObject);
    }

    protected IEnumerator ChangeColor()
    {
        npcRenderer.material.color = hitColor;
        yield return new WaitForSeconds(colorChangeDuration);
        npcRenderer.material.color = originalColor;
    }
}
