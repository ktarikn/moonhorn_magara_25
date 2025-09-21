using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour
{
    // Start is called before the first frame update
    //public BossCannon parent;
    private bool exploded = false;
    public float speed = 5f;
    public Sprite explosion;
    private Rigidbody2D rb;
    public float destroy_timer = 1f;
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
    private void OnCollisionEnter2D(Collision2D collision)
    {

       
        GetComponent<SpriteRenderer>().sprite = explosion;
        GetComponent<SpriteRenderer>().color = Color.red;
        exploded = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = true;
        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
        rb.velocity = Vector2.zero;
        Destroy(gameObject, destroy_timer);

        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //hitplayer
    }

    
}
