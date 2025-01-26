using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private int damage = 10;
   
    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IHealth>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
