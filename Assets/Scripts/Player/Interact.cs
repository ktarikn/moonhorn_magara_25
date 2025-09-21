using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D için bu kütüphane gerekli

public class Interact : MonoBehaviour
{

    [SerializeField] private GameObject eye_;
    // Inspector'dan sürükleyip býrakacaðýmýz Iþýk objesi
    [SerializeField] private Light2D playerLight;

    // Iþýðýn ulaþmasýný istediðimiz hedef yarýçap
    [SerializeField] private float targetRadius = 20f;

    // Yarýçapýn büyüme süresi (saniye cinsinden)
    [SerializeField] private float radiusIncreaseDuration = 2f;

    private GameObject collectibleInRange = null; // Tetikleyici alanýndaki toplanabilir obje
    private bool isLightExpanding = false; // Iþýðýn zaten büyümediðinden emin olmak için kontrol

    private void Start()
    {
        eye_.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Eðer menzilde bir obje varsa, F tuþuna basýlmýþsa ve ýþýk zaten büyümüyorsa
        if (collectibleInRange != null && Input.GetKeyDown(KeyCode.F) && !isLightExpanding)
        {
            // "Toplama" iþlemini baþlat
            CollectObject();
        }
    }

    private void CollectObject()
    {
        // Coroutine'i baþlatarak ýþýðý yavaþça büyüt
        StartCoroutine(ExpandLightRadius());

        // Toplanan objeyi sahnede pasif hale getir (yok et)
        collectibleInRange.SetActive(false);

        // Referansý temizle ki tekrar F'ye basýlmasýn
        collectibleInRange = null;

        eye_.gameObject.SetActive(true);
    }

    // Karakter bir tetikleyiciye girdiðinde çalýþýr
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Girdiði objenin etiketi "Collectible" ise
        if (other.CompareTag("Collectible"))
        {
            // Bu objeyi menzildeki obje olarak ata
            collectibleInRange = other.gameObject;
            Debug.Log("Toplanabilir objenin yanýndasýn. 'F' tuþuna bas.");
        }
    }

    // Karakter bir tetikleyiciden çýktýðýnda çalýþýr
    private void OnTriggerExit2D(Collider2D other)
    {
        // Çýktýðý obje menzildeki obje ile aynýysa
        if (other.gameObject == collectibleInRange)
        {
            // Referansý temizle
            collectibleInRange = null;
        }
    }

    // Iþýðýn yarýçapýný zamanla artýran Coroutine
    private IEnumerator ExpandLightRadius()
    {
        isLightExpanding = true;
        float elapsedTime = 0f;

        // Iþýðýn mevcut yarýçapýný al
        float startRadius = playerLight.pointLightOuterRadius;

        while (elapsedTime < radiusIncreaseDuration)
        {
            // Geçen süreye göre baþlangýç ve hedef yarýçap arasýnda yumuþak bir geçiþ deðeri hesapla
            float newRadius = Mathf.Lerp(startRadius, targetRadius, elapsedTime / radiusIncreaseDuration);

            // Iþýðýn yarýçapýný güncelle
            playerLight.pointLightOuterRadius = newRadius;

            // Zamaný bir sonraki kare için artýr
            elapsedTime += Time.deltaTime;

            // Bir sonraki kareye kadar bekle
            yield return null;
        }

        // Süre bittiðinde yarýçapýn tam olarak hedef deðere ulaþtýðýndan emin ol
        playerLight.pointLightOuterRadius = targetRadius;
        isLightExpanding = false; // Ýþlem bitti
    }
}