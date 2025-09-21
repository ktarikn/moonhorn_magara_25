using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Interact : MonoBehaviour
{
    [Header("I��k Ayarlar� (Sadece G�z i�in)")]
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float targetRadius = 20f;
    [SerializeField] private float radiusIncreaseDuration = 2f;

    [Header("Aktif Edilecek Par�alar")]
    [Tooltip("G�z objesi topland���nda aktif olacak child obje")]
    [SerializeField] private GameObject eyePart;
    [Tooltip("Teker objesi topland���nda aktif olacak child obje")]
    [SerializeField] private GameObject wheelPart;
    [Tooltip("M�knat�s objesi topland���nda aktif olacak child obje")]
    [SerializeField] private GameObject magnetPart;
    [Tooltip("Silah objesi topland���nda aktif olacak child obje")]
    [SerializeField] private GameObject weaponPart;
    [Tooltip("Drone objesi topland���nda aktif olacak child obje")]
    [SerializeField] private GameObject dronePart;

    private Items_enum itemInRange = null; // Menzildeki objenin CollectibleItem script'i
    private bool isLightExpanding = false;      // I��k b�y�me animasyonunu kontrol eder
    private bool hasCollectedEye = false;       // G�z'�n daha �nce al�n�p al�nmad���n� kontrol eder

    private void Update()
    {
        // Menzilde bir obje varsa ve F'ye bas�lm��sa
        if (itemInRange != null && Input.GetKeyDown(KeyCode.F))
        {
            // O objeyi "topla"
            Collect(itemInRange);
        }
    }

    private void Collect(Items_enum item)
    {
        Debug.Log(item.itemType + " topland�!");

        // Objenin t�r�ne g�re i�lem yap
        switch (item.itemType)
        {
            case CollectibleType.Eye:
                // E�er G�z daha �nce toplanmad�ysa ve ���k b�y�m�yorsa
                if (!hasCollectedEye && !isLightExpanding)
                {
                    StartCoroutine(ExpandLightRadius());
                    hasCollectedEye = true; // G�z'�n topland���n� i�aretle
                    if (eyePart != null) eyePart.SetActive(true);
                }
                break;

            case CollectibleType.Wheel:
                // WheelPart objesi atanm��sa onu aktif et
                if (wheelPart != null) wheelPart.SetActive(true);
                break;

            case CollectibleType.Magnet:
                if (magnetPart != null) magnetPart.SetActive(true);
                break;

            case CollectibleType.Weapon:
                if (weaponPart != null) weaponPart.SetActive(true);
                break;

            case CollectibleType.Drone:
                if (dronePart != null) dronePart.SetActive(true);
                break;
        }

        // Toplanan objeyi sahnede pasif hale getir
        item.gameObject.SetActive(false);

        // Referans� temizle
        itemInRange = null;
    }

    // Tetikleyiciye girme ve ��kma mant��� de�i�iyor
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Girdi�i objenin "Collectible" etiketine sahip olup olmad���n� kontrol et
        if (other.CompareTag("Collectible"))
        {
            // Objenin �zerindeki CollectibleItem script'ini al
            itemInRange = other.GetComponent<Items_enum>();
            if (itemInRange != null)
            {
                Debug.Log(itemInRange.itemType + " menzilde. 'F' tu�una bas.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // E�er menzilden ��kan obje, bizim referans tuttu�umuz obje ise
        if (itemInRange != null && other.gameObject == itemInRange.gameObject)
        {
            itemInRange = null;
        }
    }

    // I��k b�y�tme Coroutine'i ayn� kal�yor
    private IEnumerator ExpandLightRadius()
    {
        isLightExpanding = true;
        float elapsedTime = 0f;
        float startRadius = playerLight.pointLightOuterRadius;

        while (elapsedTime < radiusIncreaseDuration)
        {
            float newRadius = Mathf.Lerp(startRadius, targetRadius, elapsedTime / radiusIncreaseDuration);
            playerLight.pointLightOuterRadius = newRadius;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerLight.pointLightOuterRadius = targetRadius;
        isLightExpanding = false;
    }
}