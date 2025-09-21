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

        // Collider'ý lazer çizgisine uyacak þekilde ayarla
       // UpdateColliderForLine();
    }

    // Bu metot, BoxCollider2D'yi Line Renderer'ýn pozisyonuna,
    // boyutuna ve açýsýna göre otomatik olarak ayarlar.
   /* private void UpdateColliderForLine()
    {
        // Line Renderer'daki baþlangýç ve bitiþ noktalarýný al
        Vector3 startPoint = lineRenderer.GetPosition(0);
        Vector3 endPoint = lineRenderer.GetPosition(1);

        // Ýki nokta arasýndaki merkezi bul (collider'ýn merkezi olacak)
        // Transform'un pozisyonunu bu merkeze ayarlýyoruz
        transform.position = (startPoint + endPoint) / 2;

        // Ýki nokta arasýndaki mesafeyi bul (collider'ýn X boyutu olacak)
        float lineLength = Vector3.Distance(startPoint, endPoint);

        // BoxCollider'ýn boyutunu ayarla
        // X boyutu lazerin uzunluðu, Y boyutu lazerin kalýnlýðý olacak
        boxCollider.size = new Vector2(lineLength, lineRenderer.startWidth);

        // Lazerin açýsýný hesapla ve objeyi o yöne döndür
        Vector3 direction = (endPoint - startPoint).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }*/

    // --- DIÞARIDAN KONTROL ÝÇÝN METOTLAR ---

 /*   public void ActivateLaser()
    {
        // Lazerin hem görselini hem de collider'ýný aktif et
        lineRenderer.enabled = true;
        boxCollider.enabled = true;
    }*/

  /*  public void DeactivateLaser()
    {
        // Lazerin hem görselini hem de collider'ýný kapat
        lineRenderer.enabled = false;
        boxCollider.enabled = false;
    }*/

    // Hasar verme mantýðý
  /*  private void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer lazer aktifse ve temas eden obje oyuncu ise
        if (lineRenderer.enabled && other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu LAZER ile hasar aldý!");
            // Buraya oyuncunun canýný azaltan kodu ekleyebilirsiniz.
            // Örneðin: other.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }*/
}