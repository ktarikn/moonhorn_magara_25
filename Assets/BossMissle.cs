using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMissle : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform player;
    private bool lockedOn = false;
    public float maxDistance = 10f;
    public LayerMask hitLayers = Physics2D.DefaultRaycastLayers;
    LineRenderer lr;

    void Start()
    {
        //Line (laser)
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;

        // set meterial (check null in case we add one)
        if (lr.material == null)
        {
            lr.material = new Material(Shader.Find("Sprites/Default"));
        }

        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;

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
            yield return StartCoroutine(FireLaser(1f, 1f));

            // --- 0.5 second on / 0.5 second off ---
            yield return StartCoroutine(FireLaser(0.5f, 0.5f));

            // --- 0.3 second on / 0.3 second off ---
            yield return StartCoroutine(FireLaser(0.3f, 0.3f));

            // --- 0.2 second on / 0.2 second off ---
            yield return StartCoroutine(FireLaser(0.2f, 0.2f));

            // --- 0.2 second on / 0.2 second off ---
            yield return StartCoroutine(FireLaser(0.2f, 0.2f));

            // --- 1 second on then fire rocket ---
            //yield return StartCoroutine(FireLaser(2f, 2f));
            yield return Locked(2f,1f);
            FireRocket();
        }
    }

    System.Collections.IEnumerator Locked(float onTime, float offTime)
    {
        // Laser ON
        
        UpdateLaser();
        lr.enabled = true;
        lockedOn = true;
        float timer = 0f;
        while (timer < onTime)
        {
            
            timer += Time.deltaTime;
            yield return null;
        }
        lr.enabled = false;
        if (offTime > 0f)
        {
            yield return new WaitForSeconds(offTime);
        }
        lockedOn = false;
    }
    System.Collections.IEnumerator FireLaser(float onTime, float offTime)
    {
        // Laser ON
        float timer = 0f;
        while (timer < onTime)
        {
            UpdateLaser();
            timer += Time.deltaTime;
            yield return null;
        }

        // Laser OFF
        lr.enabled = false;
        if (offTime > 0f)
        {
            yield return new WaitForSeconds(offTime);
        }
    }

    void UpdateLaser()
    {


        lr.enabled = true;

        Vector2 origin = transform.GetChild(0).position;
        
        Vector2 direction = transform.right; // or transform.up if sprite points up
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);

        lr.SetPosition(0, origin);

        if (hit.collider != null)
        {
            Debug.Log(hit.ToString());
            lr.SetPosition(1, hit.point);
        }
        else
        {
            lr.SetPosition(1, origin + direction * maxDistance);
        }
    }

    void FireRocket()
    {
        lr.enabled = true;
        return;
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

    void LaserSeek()
    {
        Vector2 origin = transform.GetChild(0).position;
        Vector2 direction = transform.right;
        Debug.Log(transform.GetChild(0).name);
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        // Cast a ray from the object in its facing direction
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxDistance);
        // Start point always at origin
        lr.SetPosition(0, origin);

        if (hit.collider != null)
        {
            // End point at collision
            lr.SetPosition(1, hit.point);
        }
        else
        {
            // End point at max distance
            lr.SetPosition(1, origin + direction * maxDistance);
        }
        Debug.Log("should've drawn");
    }
}
