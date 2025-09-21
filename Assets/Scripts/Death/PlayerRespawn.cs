using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerRespawn : MonoBehaviour
{
    [Header("Respawn / Hit visual")]
    [SerializeField] private float respawnFreezeTime = 0.5f; // durma s�resi
    [SerializeField] private Color hitColor = Color.red;     // hasar rengi

    private Vector3 lastCheckpointPosition;
    private bool isRespawning = false;
    private bool isTakingDamage = false;

    public int health = 3;

    // cache
    private Rigidbody2D rb;
    private SpriteRenderer[] spriteRenderers;

    // geri y�kleme verileri
    private Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>();
    private Dictionary<Behaviour, bool> behaviourOriginalState = new Dictionary<Behaviour, bool>();

    private void Start()
    {
        lastCheckpointPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    // checkpoint setleme (Checkpoint script'i bunu �a��r�r)
    public void SetCheckpoint(Vector3 checkpointPos)
    {
        lastCheckpointPosition = checkpointPos;
    }

    // trap vs ba�ka yerden �a�r�
    public void Respawn()
    {
        if (!isRespawning)
            RespawnToCheckpoint();
    }

    public void TakeDamage()
    {
        if (!isTakingDamage)
        {
            StartCoroutine(Damage());
        }
    }



    private void RespawnToCheckpoint()
    {
        isRespawning = true;

        // 1) fiziksel olarak dondur
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false; // fizik sim�lasyonunu durdur -> pozisyon sabit kal�r
        }

        // 2) oyuncuya ba�l� behaviour'lar� (movement, animator vb.) devre d��� b�rak
        behaviourOriginalState.Clear();
        Behaviour[] behaviours = GetComponents<Behaviour>(); // Animator, custom scripts vb. buraya girer
        foreach (var b in behaviours)
        {
            if (b == this) continue; // bu script'i kapatma
            behaviourOriginalState[b] = b.enabled;
            b.enabled = false;
        }

        StartCoroutine(Damage());

        // 6) oyuncuyu checkpoint'e ta��
        transform.position = new Vector3(lastCheckpointPosition.x, lastCheckpointPosition.y, transform.position.z);

        // 7) behaviour'lar� eski durumuna d�nd�r
        foreach (var kv in behaviourOriginalState)
        {
            if (kv.Key != null)
                kv.Key.enabled = kv.Value;
        }

        // 8) fizi�i tekrar a�
        if (rb != null)
        {
            rb.simulated = true;
            rb.velocity = Vector2.zero;
        }

        isRespawning = false;
    }

    private IEnumerator Damage()
    {
        // 3) sprite'lar� k�rm�z� yap
        isTakingDamage = true;

        originalColors.Clear();
        foreach (var sr in spriteRenderers)
        {
            originalColors[sr] = sr.color;
            sr.color = hitColor;
        }

        // (�stersen burada ses/particles tetikleyebilirsin)

        health--;
        if(health <=0)
        {
            //�l�m
        }

        // 4) bekle (hasar g�r�n�m�)
        yield return new WaitForSeconds(respawnFreezeTime);

        // 5) renkleri geri d�nd�r
        foreach (var kv in originalColors)
        {
            if (kv.Key != null)
                kv.Key.color = kv.Value;
        }

        isTakingDamage = false;
    }
}
