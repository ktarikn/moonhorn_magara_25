using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLaserController : MonoBehaviour
{
    Laser_Controller laser;
    public float OnInterval = 3f;
    public float OffInterval = 3f;
    // Start is called before the first frame update
    void Start()
    {
        laser = GetComponent<Laser_Controller>();

        StartCoroutine(LaserPattern());
    }

    // Update is called once per frame

    System.Collections.IEnumerator LaserPattern()
    {
        while (true)
        {
            // --- 1 second on / 1 second off ---
            yield return StartCoroutine(wait(OnInterval));
            laser.DeactivateLaser();

            yield return StartCoroutine(wait(OffInterval));
            laser.ActivateLaser();
           

        }
    }
    System.Collections.IEnumerator wait(float onTime)
    {
        // Laser ON
        float timer = 0f;
        while (timer < onTime)
        {

            timer += Time.deltaTime;
            yield return null;
        }
    }
        void Update()
    {
        
    }
}
