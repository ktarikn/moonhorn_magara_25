using UnityEngine;

public class plate_detect : MonoBehaviour
{
    // Ana kontrol script'ine referans
    private Plate plateController;

    void Start()
    {
        // Hiyerarþide yukarý çýkarak ana script'i bul
        plateController = GetComponentInParent<Plate>();
        if (plateController == null)
        {
            Debug.LogError("Bu objenin üst ebeveyninde PressurePlateController script'i bulunamadý!");
        }
    }

    // Bu trigger alanýna bir nesne girdiðinde
    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && plateController != null)
        {
            // Ana script'e nesnenin eklendiðini haber ver
            plateController.AddObject(rb);
        }
    }

    // Bu trigger alanýndan bir nesne çýktýðýnda
    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && plateController != null)
        {
            // Ana script'e nesnenin çýkarýldýðýný haber ver
            plateController.RemoveObject(rb);
        }
    }
}