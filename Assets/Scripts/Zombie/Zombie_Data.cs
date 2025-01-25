using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie_Data : NPC_Data
{
    public float attackRange = 10f;
    public float attackDamage = 25f;

    public override void  TakeDamage(float damage, Vector3 attackerPosition)
    {
        base.TakeDamage(damage, attackerPosition);

        if (currentHealth > 0)
        {
            animator.SetTrigger("Hit");
        }
        
    }
    private void OnZombieAttack()
    {
        if (isDead) return;

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 rayDirection = transform.forward + Vector3.down * 0.3f;

        Debug.DrawRay(rayOrigin, rayDirection * attackRange, Color.red, 2.0f);

        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, attackRange))
        {
            
            Character charHealth = hit.collider.GetComponent<Character>();
            
            if (charHealth != null)
            {
                charHealth.TakeDamage(attackDamage, transform.position);
            }
            
        }
        else
        {
            Debug.Log("Raycast didn't hit anything.");
        }
    }


}
