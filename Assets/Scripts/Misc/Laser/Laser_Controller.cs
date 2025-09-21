using UnityEngine;


public class Laser_Controller : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private GameObject particle;

    void Awake()
    {
        // Gerekli component'leri al
        lineRenderer = GetComponent<LineRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

    }


    // --- DIÞARIDAN KONTROL ÝÇÝN METOTLAR ---

    public void ActivateLaser()
    {
        // Lazerin hem görselini hem de collider'ýný aktif et
        lineRenderer.enabled = true;
        boxCollider.enabled = true;
        particle.gameObject.SetActive(true);
    }

    public void DeactivateLaser()
    {
        // Lazerin hem görselini hem de collider'ýný kapat
        lineRenderer.enabled = false;
        boxCollider.enabled = false;
        particle.gameObject.SetActive(false);
    }

    // Hasar verme mantýðý
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Eðer lazer aktifse ve temas eden obje oyuncu ise
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu LAZER ile hasar aldý!");
            // Buraya oyuncunun canýný azaltan kodu ekleyebilirsiniz.
            // Örneðin: other.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }
}