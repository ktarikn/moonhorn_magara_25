using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePlatform : MonoBehaviour
{
    [Tooltip("Platformun nesneleri ne kadar yükseðe zýplatacaðýný belirler.")]
    public float jumpingPower = 10f;
    public float jumpingPush = 0.7f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            float horizontalhelp = jumpingPush;
            if(rb.velocity.x > 0f)
            {
                horizontalhelp *= -1f;
            }
            rb.velocity = new Vector2(rb.velocity.x + horizontalhelp, jumpingPower);
        }
    }

}
