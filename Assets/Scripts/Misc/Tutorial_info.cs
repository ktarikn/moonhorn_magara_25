using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_info : MonoBehaviour
{
    [SerializeField] private GameObject info_;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            info_.SetActive(true);
            info_.transform.Translate(0, .5f, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            info_.SetActive(false);
        }
    }
}
