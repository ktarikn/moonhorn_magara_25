using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn / Hit visual")]
    [SerializeField] private float respawnFreezeTime = 0.5f; // durma süresi
    [SerializeField] private Color hitColor = Color.red;     // hasar rengi

    private Vector3 lastCheckpointPosition;
    private bool isRespawning = false;

    // cache
    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;

    // geri yükleme verileri
    private Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();
    private Dictionary<Behaviour, bool> behaviourOriginalState = new Dictionary<Behaviour, bool>();

    private void Start()
    {
        lastCheckpointPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // checkpoint setleme (Checkpoint script'i bunu çaðýrýr)
    public void SetCheckpoint(Vector3 checkpointPos)
    {
        lastCheckpointPosition = checkpointPos;
    }

    // trap vs baþka yerden çaðrý
    public void Respawn()
    {
        if (!isRespawning)
            StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        isRespawning = true;

        // 1) fiziksel olarak dondur
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false; // fizik simülasyonunu durdur -> pozisyon sabit kalýr
        }

        // 2) oyuncuya baðlý behaviour'larý (movement, animator vb.) devre dýþý býrak
        behaviourOriginalState.Clear();
        Behaviour[] behaviours = GetComponents<Behaviour>(); // Animator, custom scripts vb. buraya girer
        foreach (var b in behaviours)
        {
            if (b == this) continue; // bu script'i kapatma
            behaviourOriginalState[b] = b.enabled;
            b.enabled = false;
        }

        // 3) sprite'larý kýrmýzý yap
        originalColors.Clear();
        foreach (var sr in spriteRenderers)
        {
            originalColors[sr] = sr.color;
            sr.color = hitColor;
        }

        // (Ýstersen burada ses/particles tetikleyebilirsin)

        // 4) bekle (hasar görünümü)
        yield return new WaitForSeconds(respawnFreezeTime);

        // 5) renkleri geri döndür
        foreach (var kv in originalColors)
        {
            if (kv.Key != null)
                kv.Key.color = kv.Value;
        }

        // 6) oyuncuyu checkpoint'e taþý
        transform.position = new Vector3(lastCheckpointPosition.x, lastCheckpointPosition.y, transform.position.z);

        // 7) behaviour'larý eski durumuna döndür
        foreach (var kv in behaviourOriginalState)
        {
            if (kv.Key != null)
                kv.Key.enabled = kv.Value;
        }

        // 8) fiziði tekrar aç
        if (rb != null)
        {
            rb.simulated = true;
            rb.velocity = Vector2.zero;
        }

        isRespawning = false;
    }
}
