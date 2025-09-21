using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Interact : MonoBehaviour
{
    [Header("Iþýk Ayarlarý (Sadece Göz için)")]
    [SerializeField] private Light2D playerLight;
    [SerializeField] private float targetRadius = 20f;
    [SerializeField] private float radiusIncreaseDuration = 2f;

    [Header("Aktif Edilecek Parçalar")]
    [Tooltip("Göz objesi toplandýðýnda aktif olacak child obje")]
    [SerializeField] private GameObject eyePart;
    [Tooltip("Teker objesi toplandýðýnda aktif olacak child obje")]
    [SerializeField] private GameObject wheelPart;
    [Tooltip("Mýknatýs objesi toplandýðýnda aktif olacak child obje")]
    [SerializeField] private GameObject magnetPart;
    [Tooltip("Silah objesi toplandýðýnda aktif olacak child obje")]
    [SerializeField] private GameObject weaponPart;
    [Tooltip("Drone objesi toplandýðýnda aktif olacak child obje")]
    [SerializeField] private GameObject dronePart;

    private Items_enum itemInRange = null; // Menzildeki objenin CollectibleItem script'i
    private bool isLightExpanding = false;      // Iþýk büyüme animasyonunu kontrol eder
    private bool hasCollectedEye = false;       // Göz'ün daha önce alýnýp alýnmadýðýný kontrol eder

    private void Update()
    {
        // Menzilde bir obje varsa ve F'ye basýlmýþsa
        if (itemInRange != null && Input.GetKeyDown(KeyCode.F))
        {
            // O objeyi "topla"
            Collect(itemInRange);
        }
    }

    private void Collect(Items_enum item)
    {
        Debug.Log(item.itemType + " toplandý!");

        // Objenin türüne göre iþlem yap
        switch (item.itemType)
        {
            case CollectibleType.Eye:
                // Eðer Göz daha önce toplanmadýysa ve ýþýk büyümüyorsa
                if (!hasCollectedEye && !isLightExpanding)
                {
                    StartCoroutine(ExpandLightRadius());
                    hasCollectedEye = true; // Göz'ün toplandýðýný iþaretle
                    if (eyePart != null) eyePart.SetActive(true);
                }
                break;

            case CollectibleType.Wheel:
                // WheelPart objesi atanmýþsa onu aktif et
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

        // Referansý temizle
        itemInRange = null;
    }

    // Tetikleyiciye girme ve çýkma mantýðý deðiþiyor
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Girdiði objenin "Collectible" etiketine sahip olup olmadýðýný kontrol et
        if (other.CompareTag("Collectible"))
        {
            // Objenin üzerindeki CollectibleItem script'ini al
            itemInRange = other.GetComponent<Items_enum>();
            if (itemInRange != null)
            {
                Debug.Log(itemInRange.itemType + " menzilde. 'F' tuþuna bas.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Eðer menzilden çýkan obje, bizim referans tuttuðumuz obje ise
        if (itemInRange != null && other.gameObject == itemInRange.gameObject)
        {
            itemInRange = null;
        }
    }

    // Iþýk büyütme Coroutine'i ayný kalýyor
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