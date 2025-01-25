using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using StarterAssets;


public class Character : MonoBehaviour
{
    public static Character Instance;
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;

    public float MaxHealth { get; private set; } = 100f;
    public float Health { get; private set; }
    public Animator animator;
    private float lastYPosition;
    public AudioClip damageHurtVoice;
    private AudioSource Audio;
    [SerializeField] private CharacterController characterController;

    private Vector3 lastPosition;
    private bool isDead = false;
    private bool isCrouch;

    private void Awake()
    {
        // GameManager'in tekil olmasını sağla
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne ge�i�lerinde yok olmas�n� engeller
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        Health = MaxHealth;

        lastYPosition = transform.position.y;
        Audio = GetComponent<AudioSource>();
        Debug.Log("prevHe�ight:" + lastYPosition);
        Debug.Log("maxh:" + MaxHealth);
        Debug.Log("health:" + Health);
    }

    void Update()
    {
        //IncreaseHealth();
    }

    public void TakeDamage(float damage, Vector3 attackerPosition) // karakterin canını düşüren fonksiyon
    {
        if (isDead) return;
        Audio.clip = damageHurtVoice;
        Audio.Play();
        Health -= damage;
        lastPosition = attackerPosition;
        animator.SetTrigger("ZombieHit");

        OnHealthChanged?.Invoke(Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    public void IncreaseHealth()
    {
        if (isDead) return;
                Debug.Log("ıncrsesaaaa ");
                Health = Mathf.Min(MaxHealth, Health - 10);
                OnHealthChanged?.Invoke(Health);
     
    }



    private void Die()
    {
        isDead = true;
        OnDeath?.Invoke();

        // Hareketi durdur
        if (characterController != null)
        {
            characterController.enabled = false;

        }


        // �l�m animasyonu tetikle
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Debug.Log("Character has died.");

    }

    
    public void OnDeathAnimationEnd()
    {
        GameManager.Instance.GameOver();
    }

}


