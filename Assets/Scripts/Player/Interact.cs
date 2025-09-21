using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Light2D i�in bu k�t�phane gerekli

public class Interact : MonoBehaviour
{

    [SerializeField] private GameObject eye_;
    // Inspector'dan s�r�kleyip b�rakaca��m�z I��k objesi
    [SerializeField] private Light2D playerLight;

    // I����n ula�mas�n� istedi�imiz hedef yar��ap
    [SerializeField] private float targetRadius = 20f;

    // Yar��ap�n b�y�me s�resi (saniye cinsinden)
    [SerializeField] private float radiusIncreaseDuration = 2f;

    private GameObject collectibleInRange = null; // Tetikleyici alan�ndaki toplanabilir obje
    private bool isLightExpanding = false; // I����n zaten b�y�medi�inden emin olmak i�in kontrol

    private void Start()
    {
        eye_.gameObject.SetActive(false);
    }

    private void Update()
    {
        // E�er menzilde bir obje varsa, F tu�una bas�lm��sa ve ���k zaten b�y�m�yorsa
        if (collectibleInRange != null && Input.GetKeyDown(KeyCode.F) && !isLightExpanding)
        {
            // "Toplama" i�lemini ba�lat
            CollectObject();
        }
    }

    private void CollectObject()
    {
        // Coroutine'i ba�latarak ����� yava��a b�y�t
        StartCoroutine(ExpandLightRadius());

        // Toplanan objeyi sahnede pasif hale getir (yok et)
        collectibleInRange.SetActive(false);

        // Referans� temizle ki tekrar F'ye bas�lmas�n
        collectibleInRange = null;

        eye_.gameObject.SetActive(true);
    }

    // Karakter bir tetikleyiciye girdi�inde �al���r
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Girdi�i objenin etiketi "Collectible" ise
        if (other.CompareTag("Collectible"))
        {
            // Bu objeyi menzildeki obje olarak ata
            collectibleInRange = other.gameObject;
            Debug.Log("Toplanabilir objenin yan�ndas�n. 'F' tu�una bas.");
        }
    }

    // Karakter bir tetikleyiciden ��kt���nda �al���r
    private void OnTriggerExit2D(Collider2D other)
    {
        // ��kt��� obje menzildeki obje ile ayn�ysa
        if (other.gameObject == collectibleInRange)
        {
            // Referans� temizle
            collectibleInRange = null;
        }
    }

    // I����n yar��ap�n� zamanla art�ran Coroutine
    private IEnumerator ExpandLightRadius()
    {
        isLightExpanding = true;
        float elapsedTime = 0f;

        // I����n mevcut yar��ap�n� al
        float startRadius = playerLight.pointLightOuterRadius;

        while (elapsedTime < radiusIncreaseDuration)
        {
            // Ge�en s�reye g�re ba�lang�� ve hedef yar��ap aras�nda yumu�ak bir ge�i� de�eri hesapla
            float newRadius = Mathf.Lerp(startRadius, targetRadius, elapsedTime / radiusIncreaseDuration);

            // I����n yar��ap�n� g�ncelle
            playerLight.pointLightOuterRadius = newRadius;

            // Zaman� bir sonraki kare i�in art�r
            elapsedTime += Time.deltaTime;

            // Bir sonraki kareye kadar bekle
            yield return null;
        }

        // S�re bitti�inde yar��ap�n tam olarak hedef de�ere ula�t���ndan emin ol
        playerLight.pointLightOuterRadius = targetRadius;
        isLightExpanding = false; // ��lem bitti
    }
}