using System;
using UnityEngine;

public interface IHealth
{
    void TakeDamage(int damage);
}

public class EnemyHealth : MonoBehaviour, IHealth
{
    public float maxHealth = 10f;
    private float currentHealth;
    [SerializeField] private GameObject bubblePrefab;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            SpawnBubble();
            Destroy(gameObject);
        }
    }


    private void SpawnBubble()
    {
        if (bubblePrefab != null)
        {
            Instantiate(bubblePrefab, transform.position, Quaternion.identity); // Düþmanýn olduðu konumda bubble spawn edilir.
        }
        else
        {
            Debug.LogWarning("Bubble prefab is not assigned!");
        }
    }
}
