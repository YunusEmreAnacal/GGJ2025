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
    public event Action<float> OnFoodChanged;
    public event Action<float> OnWaterChanged;
    public event Action OnDeath;

    public float MaxHealth { get; private set; } = 100f;

    public float Health { get; private set; }

    public float MaxFood { get; private set; } = 100f;

    public float Food { get; private set; }

    public float MaxWater { get; private set; } = 100f;

    public float Water { get; private set; }


    public Animator animator;

    private float lastYPosition;
    private bool isFalling = false;

    private bool isFallDamage = false;
    public float minFallHeightForDamage = 3f;
    public float fallDamageMultiplier = 0.1f; // Y�ksekli�e ba�l� hasar �arpan�

    private float hungerTimer = 0f;
    private float healthTimer = 0f;
    private float thirstTimer = 0f;

    public float hungerTime = 10f;
    public float thirstTime = 5f;
    public float healthIncreaseTime = 3f;

    public AudioClip damageHurtVoice;
    public AudioClip drinkVoice;

    public AudioClip swimmingVoice;
    private AudioSource Audio;

    [SerializeField] private CharacterController characterController;
    private ThirdPersonController tpcontroller;

    private Vector3 lastPosition;
    private bool isDead = false;
    private bool isCrouch;
    public bool inWater;

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
        Food = MaxFood;
        Water = MaxWater;

        lastYPosition = transform.position.y;
        Audio = GetComponent<AudioSource>();
        Debug.Log("prevHe�ight:" + lastYPosition);
        Debug.Log("maxh:" + MaxHealth);
        Debug.Log("health:" + Health);
    }

    void Update()
    {
        
        ReduceHunger();
        ReduceThirsty();
        IncreaseHealth();
        Debug.Log("water:" + inWater);
        

        // Karakterin düşüşee geçtiğini belirle
        if (characterController.isGrounded)
        {
            if (isFalling)
            {
                
                CalculateFallDamage();
                isFalling = false;
            }
        }
        else
        {
            if (!isFalling)
            {
                isFalling = true;
                lastYPosition = transform.position.y; // Düşüş başlamadan önceki yüksekliği kaydet
            }
        }
    }

    private void CalculateFallDamage() // fallDamage ile can azalma hesaplaması 
    {
        if (inWater) return;

        float currentYPosition = transform.position.y;
        float fallHeight = lastYPosition - currentYPosition; // D���� y�ksekli�ini hesapla

        if (fallHeight > minFallHeightForDamage) // D���� y�ksekli�i pozitifse hasar uygula
        {
            float fallDamage = (fallHeight - minFallHeightForDamage) * fallDamageMultiplier; // D����e ba�l� hasar� hesapla
            isFallDamage = true;//parametre
            TakeDamage(fallDamage, lastPosition); // Hesaplanan hasar� uygula
                
                
        }
        
        isFallDamage=false;
    }

    public void TakeDamage(float damage, Vector3 attackerPosition) // karakterin canını düşüren fonksiyon
    {
        if (isDead) return;
        Audio.clip = damageHurtVoice;
        Audio.Play();
        Health -= damage;
        lastPosition = attackerPosition;
        if (!isFallDamage) animator.SetTrigger("ZombieHit");



        OnHealthChanged?.Invoke(Health);

        if (Health <= 0)
        {
            Die();
        }
    }

    private void IncreaseHealth()
    {
        if (isDead) return;
        healthTimer += Time.deltaTime;
        if (healthTimer > healthIncreaseTime)
        {
            if (Food >= MaxFood && Health < MaxHealth)
            {
                Debug.Log("ıncrsesaaaa ");
                Health = Mathf.Min(MaxHealth, Health + 10);
                OnHealthChanged?.Invoke(Health);
                healthTimer = 0f;
            }

        }


    }



    private void ReduceHungerAndThirsty(ref float timer, float timerInterval, ref float value, Action<float> onValueChanged,int damage) // tekilleştirme
    {

        if (isDead) return;

        timer += Time.deltaTime;
        if (timer >= timerInterval)
        {
            if (value > 0)
            {
                Debug.Log("xxx");
                value = Mathf.Max(0, value - 10);
                onValueChanged?.Invoke(value);

            }
            else
            {
                TakeDamage(damage,lastPosition);

            }
            timer = 0f;
        }
    }

    private void ReduceThirsty()
    {
        float currentWater = Water;
        ReduceHungerAndThirsty(ref thirstTimer,thirstTime,ref currentWater,OnWaterChanged, 10);
        Water = currentWater;
        
    }
    private void ReduceHunger()
    {
        float currentFood = Food;
        ReduceHungerAndThirsty(ref hungerTimer,hungerTime,ref currentFood,OnFoodChanged, 20);
        Food = currentFood;
        
    }

    public void IncreaseFood(float foodIncrease)
    {
        if (isDead) return;
        if (Food < MaxFood)
        {
            Food = Mathf.Min(MaxFood, foodIncrease + Food);
            OnFoodChanged?.Invoke(Food);
            Water = Mathf.Max(0, Water - 10);
            OnWaterChanged?.Invoke(Water);
        }

    }

    public void OnIncreaseThirst()
    {
        if (isDead) return;
        Water = Mathf.Min(MaxWater, 100f + Water); // parametre
        OnWaterChanged?.Invoke(Water);
        Audio.clip = drinkVoice;
        Audio.Play();
        thirstTimer = 0f;
         
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

    

    public void OnSwimming()
    {
        Audio.clip = swimmingVoice;
        Audio.PlayOneShot(swimmingVoice);

    }

    public void OnDeathAnimationEnd()
    {
        GameManager.Instance.GameOver();
    }

}


