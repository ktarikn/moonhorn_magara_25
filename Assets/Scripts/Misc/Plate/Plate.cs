using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    // --- ESK� DE���KENLER AYNI KALIYOR ---
    [Header("G�rsel Ayarlar")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color pressedColor = Color.green;

    [Header("Hareket Ayarlar�")]
    [SerializeField] private float pressedOffsetY = -0.2f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Ba�lant�lar")]
    [SerializeField] private Transform buttonTransform;

    // --- YEN� DE���KEN� EKLEYEL�M ---
    [Header("Kontrol Edilecek Obje")]
    [Tooltip("Bu plaka bas�ld���nda kontrol edilecek kap�")]
    [SerializeField] private Door doorToControl;

    [Tooltip("Bu plaka bas�ld���nda kontrol edilecek lazer (iste�e ba�l�)")]
    [SerializeField] private Laser_Controller laserToControl;

    private SpriteRenderer buttonSpriteRenderer;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private List<Rigidbody2D> objectsOnPlate = new();

    void Start()
    {
        // Start metodu ayn� kal�yor
        if (buttonTransform != null)
        {
            originalPosition = buttonTransform.position;
            targetPosition = originalPosition;
            buttonSpriteRenderer = buttonTransform.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Button Transform atanmam��!", this.gameObject);
        }
        SetColor();
    }

    void FixedUpdate()
    {
        // FixedUpdate metodu ayn� kal�yor
        if (objectsOnPlate.Count > 0)
        {
            targetPosition = originalPosition + new Vector3(0, pressedOffsetY, 0);
        }
        else
        {
            targetPosition = originalPosition;
        }
        buttonTransform.position = Vector3.Lerp(buttonTransform.position, targetPosition, Time.fixedDeltaTime * moveSpeed);
    }













    // --- AddObject ve RemoveObject METOTLARINI G�NCELLEYEL�M ---

    public void AddObject(Rigidbody2D rb)
    {
        if (!objectsOnPlate.Contains(rb))
        {
            // E�er bu, plakan�n �zerine konan �LK nesne ise...
            if (objectsOnPlate.Count == 0)
            {
                doorToControl?.OpenDoor();
                //laserToControl?.DeactivateLaser();
            }

            objectsOnPlate.Add(rb);
            SetColor();
        }
    }

    public void RemoveObject(Rigidbody2D rb)
    {
        if (objectsOnPlate.Contains(rb))
        {
            objectsOnPlate.Remove(rb);

            // E�er bu nesne kalkt�ktan sonra plakan�n �zerinde H�� nesne kalmad�ysa...
            if (objectsOnPlate.Count == 0)
            {
                doorToControl?.CloseDoor();
                //laserToControl?.ActivateLaser();

                SetColor();
            }
        }
    }

    private void SetColor()
    {
        // SetColor metodu ayn� kal�yor
        if (buttonSpriteRenderer == null) return;

        if (objectsOnPlate.Count > 0)
        {
            buttonSpriteRenderer.color = pressedColor;
        }
        else
        {
            buttonSpriteRenderer.color = normalColor;
        }
    }
}