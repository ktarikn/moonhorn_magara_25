using System.Collections;
using UnityEngine;

public class BossGun : MonoBehaviour
{
    // Inspector'dan kolayca ayarlanabilir de�i�kenler
    [Header("Hedefleme Ayarlar�")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private bool lockOnPlayer = true;

    [Header("Mermi Ayarlar�")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzleTransform; // Mermi ��k�� noktas�
    [SerializeField] private float bulletSpeed = 10f; // Mermi h�z�
    [SerializeField] private float shootInterval = 3f;

    [Header("At�� Deseni")]
    [SerializeField] private int numberOfBullets = 3;
    [SerializeField] private float spreadAngle = 45f;

    private Transform player;

    private void Start()
    {
        // Oyuncuyu bul ve null kontrol� yap
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Player' found!");
        }

        // At�� Coroutine'ini ba�lat
        StartCoroutine(ShootPattern());
    }

    private void Update()
    {
        // E�er kilitlenme aktifse, oyuncuya bak
        if (player != null && lockOnPlayer)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private IEnumerator ShootPattern()
    {
        // Sonsuz d�ng�de at�� yap
        while (true)
        {
            // At�� aral��� kadar bekle
            yield return new WaitForSeconds(shootInterval);

            // Mermi at�� fonksiyonunu �a��r
            FireBullets();
        }
    }

    private void FireBullets()
    {
        if (bulletPrefab == null || muzzleTransform == null || player == null)
        {
            Debug.LogWarning("Mermi prefab'�, ��k�� noktas� veya oyuncu atanmam��!");
            return;
        }

        // Oyuncuya olan temel y�n� hesapla
        Vector3 directionToPlayer = (player.position - muzzleTransform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Toplam yay�lma a��s�n� hesapla
        float totalSpread = (numberOfBullets - 1) * spreadAngle;
        float startingAngle = -totalSpread / 2;

        for (int i = 0; i < numberOfBullets; i++)
        {
            // Her mermi i�in d�nme a��s�n� hesapla
            float rotationOffset = startingAngle + i * spreadAngle;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, baseAngle + rotationOffset);

            // Mermiyi olu�tur
            GameObject newBullet = Instantiate(bulletPrefab, muzzleTransform.position, bulletRotation);

            // Mermiye h�z ver
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = newBullet.transform.right * bulletSpeed;
            }
        }
    }
}