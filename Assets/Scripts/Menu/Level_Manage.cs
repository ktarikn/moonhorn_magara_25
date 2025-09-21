using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_Manage : MonoBehaviour
{
    [SerializeField] private int level_index;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Load_Next_Level();
    }

    public void Load_Next_Level()
    {
        // int currentLevel = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(level_index);
    }
}
