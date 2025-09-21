using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Boss_Enter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float transitionDuration = 2f; // Geçiþin ne kadar süreceði (saniye cinsinden)

    private CinemachineTransposer transposer;

    private void Start()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Coroutine'i baþlat
            StartCoroutine(SmoothCameraTransition(15f, 5f));
        }
    }

    private IEnumerator SmoothCameraTransition(float targetOrthographicSize, float targetFollowOffsetY)
    {
        // Baþlangýç deðerlerini al
        float startOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
        float startFollowOffsetY = transposer.m_FollowOffset.y;
        float elapsedTime = 0f;

        // Hedef deðerlere ulaþana kadar döngüyü çalýþtýr
        while (elapsedTime < transitionDuration)
        {
            // Oran hesapla (0 ile 1 arasýnda)
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            // OrthographicSize deðerini yavaþça artýr
            virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startOrthographicSize, targetOrthographicSize, t);

            // Follow Offset Y deðerini yavaþça artýr
            Vector3 currentFollowOffset = transposer.m_FollowOffset;
            currentFollowOffset.y = Mathf.Lerp(startFollowOffsetY, targetFollowOffsetY, t);
            transposer.m_FollowOffset = currentFollowOffset;

            // Bir sonraki frame'i bekle
            yield return null;
        }

        // Geçiþ bittiðinde deðerlerin tam olarak hedefe ulaþmasýný garantile
        virtualCamera.m_Lens.OrthographicSize = targetOrthographicSize;
        Vector3 finalFollowOffset = transposer.m_FollowOffset;
        finalFollowOffset.y = targetFollowOffsetY;
        transposer.m_FollowOffset = finalFollowOffset;
    }
}