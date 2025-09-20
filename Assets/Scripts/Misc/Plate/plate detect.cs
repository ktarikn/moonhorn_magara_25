using UnityEngine;

public class plate_detect : MonoBehaviour
{
    // Ana kontrol script'ine referans
    private Plate plateController;

    void Start()
    {
        // Hiyerar�ide yukar� ��karak ana script'i bul
        plateController = GetComponentInParent<Plate>();
        if (plateController == null)
        {
            Debug.LogError("Bu objenin �st ebeveyninde PressurePlateController script'i bulunamad�!");
        }
    }

    // Bu trigger alan�na bir nesne girdi�inde
    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && plateController != null)
        {
            // Ana script'e nesnenin eklendi�ini haber ver
            plateController.AddObject(rb);
        }
    }

    // Bu trigger alan�ndan bir nesne ��kt���nda
    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null && plateController != null)
        {
            // Ana script'e nesnenin ��kar�ld���n� haber ver
            plateController.RemoveObject(rb);
        }
    }
}