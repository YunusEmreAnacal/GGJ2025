using UnityEngine;

public class CharactherMouseLook : MonoBehaviour
{
    public Camera mainCamera; // Oyun kameran� buraya s�r�kle veya scriptte ata.

    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // Mouse'un ekran �zerindeki pozisyonunu al.
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Mouse pozisyonunu oyun d�nyas�ndaki bir noktaya d�n��t�r.
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // XZ d�zleminde bir plane olu�tur.

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(enter); // Mouse'un d�nya �zerindeki pozisyonu.

            // Karakterin mouse'a do�ru bakaca�� y�n� hesapla.
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;

            // Karakteri o y�ne d�nd�r.
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Yaln�zca Y ekseninde d�n.
        }
    }
}
