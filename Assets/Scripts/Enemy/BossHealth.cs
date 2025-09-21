using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int lives = 3;
    void Start()
    {
        
    }

    // Update is called once per frame

    public void getHit()
    {
        lives--;
        if(lives == 0)
        {
            //level_end()
        }
    }
    void Update()
    {
        
    }
}
