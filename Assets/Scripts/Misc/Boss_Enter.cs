using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss_Enter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float transitionDuration = 2f; // Ge�i�in ne kadar s�rece�i (saniye cinsinden)

    private CinemachineTransposer transposer;

    private void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Coroutine'i ba�lat
            StartCoroutine(SmoothCameraTransition(15f, 5f));
        }
    }

    private IEnumerator SmoothCameraTransition(float targetOrthographicSize, float targetFollowOffsetY)
    {
        // Ba�lang�� de�erlerini al
        float startOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
        float startFollowOffsetY = transposer.m_FollowOffset.y;
        float elapsedTime = 0f;

        // Hedef de�erlere ula�ana kadar d�ng�y� �al��t�r
        while (elapsedTime < transitionDuration)
        {
            // Oran hesapla (0 ile 1 aras�nda)
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // OrthographicSize de�erini yava��a art�r
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startOrthographicSize, targetOrthographicSize, t);

            // Follow Offset Y de�erini yava��a art�r
            Vector3 currentFollowOffset = transposer.m_FollowOffset;
            currentFollowOffset.y = Mathf.Lerp(startFollowOffsetY, targetFollowOffsetY, t);
            transposer.m_FollowOffset = currentFollowOffset;

            // Bir sonraki frame'i bekle
            yield return null;
        }

        // Ge�i� bitti�inde de�erlerin tam olarak hedefe ula�mas�n� garantile
        virtualCamera.m_Lens.OrthographicSize = targetOrthographicSize;
        Vector3 finalFollowOffset = transposer.m_FollowOffset;
        finalFollowOffset.y = targetFollowOffsetY;
        transposer.m_FollowOffset = finalFollowOffset;
    }
}