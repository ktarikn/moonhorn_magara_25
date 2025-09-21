using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGun : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;

    public GameObject bulletPrefab;
    public Vector2 muzzle { get { return transform.GetChild(0).position; } set { } }
    private bool lockedOn = false;
    public float maxDistance = 10f;
    public LayerMask hitLayers = Physics2D.DefaultRaycastLayers;

    public float shootInterval = 3f;

    void Start()
    {
        

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No GameObject with tag 'Player' found!");
        }

        StartCoroutine(LaserPattern());
    }


    System.Collections.IEnumerator LaserPattern()
    {
        while (true)
        {
            // --- 1 second on / 1 second off ---
            yield return StartCoroutine(FireBullet(shootInterval));

            

        }
    }

    
    System.Collections.IEnumerator FireBullet(float onTime)
    {
        // Laser ON
        float timer = 0f;
        while (timer < onTime)
        {
            
            timer += Time.deltaTime;
            yield return null;
        }

        if (bulletPrefab)
            Instantiate(bulletPrefab, muzzle, transform.rotation);
            Instantiate(bulletPrefab, muzzle, transform.rotation* Quaternion.Euler(0, 0, 45f));
            Instantiate(bulletPrefab, muzzle, transform.rotation * Quaternion.Euler(0, 0, -45f));
    }

   

   


    // Update is called once per frame
    void Update()
    {
        if (player != null && !lockedOn)
        {
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }


    }

}
