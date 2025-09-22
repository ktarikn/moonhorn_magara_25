using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_damage : MonoBehaviour
{
    [SerializeField] private int damage_;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Boss"))
        {
            BossHealth boss = collision.GetComponent<BossHealth>();
            boss.getHit(damage_);
        }
    }
}
