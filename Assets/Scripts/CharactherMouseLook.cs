using UnityEngine;

public class CharactherMouseLook : MonoBehaviour
{
    public Camera mainCamera; // Oyun kameraný buraya sürükle veya scriptte ata.

    void Update()
    {
        RotateTowardsMouse();
    }

    void RotateTowardsMouse()
    {
        // Mouse'un ekran üzerindeki pozisyonunu al.
        Vector3 mouseScreenPosition = Input.mousePosition;

        // Mouse pozisyonunu oyun dünyasýndaki bir noktaya dönüþtür.
        Ray ray = mainCamera.ScreenPointToRay(mouseScreenPosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // XZ düzleminde bir plane oluþtur.

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(enter); // Mouse'un dünya üzerindeki pozisyonu.

            // Karakterin mouse'a doðru bakacaðý yönü hesapla.
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;

            // Karakteri o yöne döndür.
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0); // Yalnýzca Y ekseninde dön.
        }
    }
}
