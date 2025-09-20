using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool isRight;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector3 move = speed * Time.deltaTime * (isRight ? Vector3.right : Vector3.left);

        if (isRight)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        collision.transform.Translate(move);
    }
}
