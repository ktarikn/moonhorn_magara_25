using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    // --- ESKÝ DEÐÝÞKENLER AYNI KALIYOR ---
    [Header("Görsel Ayarlar")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color pressedColor = Color.green;

    [Header("Hareket Ayarlarý")]
    [SerializeField] private float pressedOffsetY = -0.2f;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Baðlantýlar")]
    [SerializeField] private Transform buttonTransform;

    // --- YENÝ DEÐÝÞKENÝ EKLEYELÝM ---
    [Header("Kontrol Edilecek Obje")]
    [Tooltip("Bu plaka basýldýðýnda kontrol edilecek kapý")]
    [SerializeField] private Door doorToControl;

    [Tooltip("Bu plaka basýldýðýnda kontrol edilecek lazer (isteðe baðlý)")]
    [SerializeField] private Laser_Controller laserToControl;

    private SpriteRenderer buttonSpriteRenderer;
    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private List<Rigidbody2D> objectsOnPlate = new();

    void Start()
    {
        // Start metodu ayný kalýyor
        if (buttonTransform != null)
        {
            originalPosition = buttonTransform.position;
            targetPosition = originalPosition;
            buttonSpriteRenderer = buttonTransform.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Button Transform atanmamýþ!", this.gameObject);
        }
        SetColor();
    }

    void FixedUpdate()
    {
        // FixedUpdate metodu ayný kalýyor
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













    // --- AddObject ve RemoveObject METOTLARINI GÜNCELLEYELÝM ---

    public void AddObject(Rigidbody2D rb)
    {
        if (!objectsOnPlate.Contains(rb))
        {
            // Eðer bu, plakanýn üzerine konan ÝLK nesne ise...
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

            // Eðer bu nesne kalktýktan sonra plakanýn üzerinde HÝÇ nesne kalmadýysa...
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
        // SetColor metodu ayný kalýyor
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