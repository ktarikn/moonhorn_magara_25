using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Laser_Controller : MonoBehaviour
{
   // [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private BoxCollider2D boxCollider;

    void Awake()
    {
        // Gerekli component'leri al
       // lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Collider'� lazer �izgisine uyacak �ekilde ayarla
       // UpdateColliderForLine();
    }

    // Bu metot, BoxCollider2D'yi Line Renderer'�n pozisyonuna,
    // boyutuna ve a��s�na g�re otomatik olarak ayarlar.
   /* private void UpdateColliderForLine()
    {
        // Line Renderer'daki ba�lang�� ve biti� noktalar�n� al
        Vector3 startPoint = lineRenderer.GetPosition(0);
        Vector3 endPoint = lineRenderer.GetPosition(1);

        // �ki nokta aras�ndaki merkezi bul (collider'�n merkezi olacak)
        // Transform'un pozisyonunu bu merkeze ayarl�yoruz
        transform.position = (startPoint + endPoint) / 2;

        // �ki nokta aras�ndaki mesafeyi bul (collider'�n X boyutu olacak)
        float lineLength = Vector3.Distance(startPoint, endPoint);

        // BoxCollider'�n boyutunu ayarla
        // X boyutu lazerin uzunlu�u, Y boyutu lazerin kal�nl��� olacak
        boxCollider.size = new Vector2(lineLength, lineRenderer.startWidth);

        // Lazerin a��s�n� hesapla ve objeyi o y�ne d�nd�r
        Vector3 direction = (endPoint - startPoint).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }*/

    // --- DI�ARIDAN KONTROL ���N METOTLAR ---

 /*   public void ActivateLaser()
    {
        // Lazerin hem g�rselini hem de collider'�n� aktif et
        lineRenderer.enabled = true;
        boxCollider.enabled = true;
    }*/

  /*  public void DeactivateLaser()
    {
        // Lazerin hem g�rselini hem de collider'�n� kapat
        lineRenderer.enabled = false;
        boxCollider.enabled = false;
    }*/

    // Hasar verme mant���
  /*  private void OnTriggerEnter2D(Collider2D other)
    {
        // E�er lazer aktifse ve temas eden obje oyuncu ise
        if (lineRenderer.enabled && other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu LAZER ile hasar ald�!");
            // Buraya oyuncunun can�n� azaltan kodu ekleyebilirsiniz.
            // �rne�in: other.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }*/
}