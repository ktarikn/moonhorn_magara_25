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


    // --- DI�ARIDAN KONTROL ���N METOTLAR ---

    public void ActivateLaser()
    {
        // Lazerin hem g�rselini hem de collider'�n� aktif et
        lineRenderer.enabled = true;
        boxCollider.enabled = true;
        particle.gameObject.SetActive(true);
    }

    public void DeactivateLaser()
    {
        // Lazerin hem g�rselini hem de collider'�n� kapat
        lineRenderer.enabled = false;
        boxCollider.enabled = false;
        particle.gameObject.SetActive(false);
    }

    // Hasar verme mant���
    private void OnTriggerEnter2D(Collider2D other)
    {
        // E�er lazer aktifse ve temas eden obje oyuncu ise
        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu LAZER ile hasar ald�!");
            // Buraya oyuncunun can�n� azaltan kodu ekleyebilirsiniz.
            // �rne�in: other.GetComponent<PlayerHealth>().TakeDamage(10);
        }
    }
}