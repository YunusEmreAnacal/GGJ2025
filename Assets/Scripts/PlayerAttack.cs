using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject waterBulletPrefab;
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private float shootCooldown = 0.5f;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator; // Animasyon kontrolcüsü
    [SerializeField] private string attackAnimationTrigger = "Attack";

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource; // Ses kaynaðý
    [SerializeField] private AudioClip shootSoundEffect; // Ses efekti

    private Vector2 aimPosition;
    private Camera mainCamera;
    private float nextShootTime;
    private Plane groundPlane;

    private void Start()
    {
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void OnAim(InputValue value)
    {
        aimPosition = value.Get<Vector2>();
    }

    private void OnAttack(InputValue value)
    {
        if (Time.time >= nextShootTime && value.isPressed)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        nextShootTime = Time.time + shootCooldown;

        if (animator != null)
        {
            animator.SetTrigger(attackAnimationTrigger);
        }

        
        if (audioSource != null && shootSoundEffect != null)
        {
            audioSource.PlayOneShot(shootSoundEffect);
        }

        Ray ray = mainCamera.ScreenPointToRay(aimPosition);
        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 worldAimPoint = ray.GetPoint(distance);
            Vector3 shootDirection = (worldAimPoint - transform.position).normalized;
           
            GameObject bullet = Instantiate(waterBulletPrefab, transform.position + Vector3.up, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().linearVelocity = shootDirection * bulletSpeed;
           
            bullet.transform.forward = shootDirection;
        }
    }
}