using System.Collections;
using UnityEngine;

public class BossGun : MonoBehaviour
{
    // Inspector'dan kolayca ayarlanabilir deðiþkenler
    [Header("Hedefleme Ayarlarý")]
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private LayerMask hitLayers;
    [SerializeField] private bool lockOnPlayer = true;

    [Header("Mermi Ayarlarý")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzleTransform; // Mermi çýkýþ noktasý
    [SerializeField] private float bulletSpeed = 10f; // Mermi hýzý
    [SerializeField] private float shootInterval = 3f;

    [Header("Atýþ Deseni")]
    [SerializeField] private int numberOfBullets = 3;
    [SerializeField] private float spreadAngle = 45f;

    private Transform player;

    private void Start()
    {
        // Oyuncuyu bul ve null kontrolü yap
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Player' found!");
        }

        // Atýþ Coroutine'ini baþlat
        StartCoroutine(ShootPattern());
    }

    private void Update()
    {
        // Eðer kilitlenme aktifse, oyuncuya bak
        if (player != null && lockOnPlayer)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private IEnumerator ShootPattern()
    {
        // Sonsuz döngüde atýþ yap
        while (true)
        {
            // Atýþ aralýðý kadar bekle
            yield return new WaitForSeconds(shootInterval);

            // Mermi atýþ fonksiyonunu çaðýr
            FireBullets();
        }
    }

    private void FireBullets()
    {
        if (bulletPrefab == null || muzzleTransform == null || player == null)
        {
            Debug.LogWarning("Mermi prefab'ý, çýkýþ noktasý veya oyuncu atanmamýþ!");
            return;
        }

        // Oyuncuya olan temel yönü hesapla
        Vector3 directionToPlayer = (player.position - muzzleTransform.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

        // Toplam yayýlma açýsýný hesapla
        float totalSpread = (numberOfBullets - 1) * spreadAngle;
        float startingAngle = -totalSpread / 2;

        for (int i = 0; i < numberOfBullets; i++)
        {
            // Her mermi için dönme açýsýný hesapla
            float rotationOffset = startingAngle + i * spreadAngle;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, baseAngle + rotationOffset);

            // Mermiyi oluþtur
            GameObject newBullet = Instantiate(bulletPrefab, muzzleTransform.position, bulletRotation);

            // Mermiye hýz ver
            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = newBullet.transform.right * bulletSpeed;
            }
        }
    }
}