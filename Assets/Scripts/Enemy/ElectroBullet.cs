using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectroBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3f;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.CompareTag("Player"))
        {
            PlayerRespawn respawn = collision.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                respawn.TakeDamage();
            }
            GetComponent<SpriteRenderer>().sprite = null;
            Destroy(gameObject, 0.3f);
        }
        if (collision.gameObject.CompareTag("Boundry"))
        {
            GetComponent<SpriteRenderer>().sprite = null;
            Destroy(gameObject, 0.3f);
        }
        
    }
}
