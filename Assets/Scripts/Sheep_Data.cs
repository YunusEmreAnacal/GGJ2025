using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Sheep_Data : NPC_Data
{

    public float knockbackForce = 10f;
    public GameObject mealPrefab;

    public override void  TakeDamage(float damage, Vector3 attackerPosition)
    {
        base.TakeDamage(damage, attackerPosition);

        if (currentHealth > 0)
        {
            FleeFromAttacker();
        }
        
    }


    protected override IEnumerator DieRoutine()
    {
        yield return base.DieRoutine();

        // Meal objelerinin spawn edilmesi
        int numberOfMeals = Random.Range(1, 4);
        for (int i = 0; i < numberOfMeals; i++)
        {
            Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 1.5f;
            spawnPosition.y = transform.position.y; // raycast 
            Instantiate(mealPrefab, spawnPosition, Random.rotation);
        }
    }

    private void FleeFromAttacker()
    {
        Vector3 fleeDirection = (transform.position - lastAttackerPosition).normalized * 5f;
        agent.SetDestination(transform.position + fleeDirection);
    }

}


