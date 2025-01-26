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

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);   
        if (currentHealth <= 0) Destroy(gameObject);
    }
}
