using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public Action keyAAction;
    public Action keyDAction;
    public Action keyWAction;
    public Action keySAction;

    private Rigidbody2D rb;
    public float rollSpeed = 200f; // torque gücü
    public float maxAngularSpeed = 300f; // max dönme hýzý (derece/sn)

    public float moveSpeed = 100;
    public bool canCarMove;
    public Transform carGroundCheck1;
    public Transform carGroundCheck2;
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    public bool isGrounded;

    public GameObject gun;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (canCarMove) isGrounded = Physics2D.OverlapCircle(carGroundCheck1.position, checkRadius, groundLayer) || Physics2D.OverlapCircle(carGroundCheck2.position, checkRadius, groundLayer);
        if (Input.GetKey(KeyCode.A))
        {
            if (isGrounded) rideLeft();
        }

        else if (Input.GetKey(KeyCode.D))
        {
            if (isGrounded) rideRight();
        }
        else if (rb.velocity.x != 0)
        {
            toZero();
        }

        if (Input.GetKey(KeyCode.W))
        {
            TryFly(); // uçmayý deneyelim
        }
        if (Input.GetKey(KeyCode.S))
        {
            //empty
        }
        if (Input.GetKey(KeyCode.Q))
        {
            RollLeft();
        }
        if (Input.GetKey(KeyCode.E))
        {
            RollRight();
        }
        if (Input.GetMouseButtonDown(0)) {
            if (canShoot) shoot();
        }
        if (Input.GetMouseButton(1)) // mýknatýs aktif
        {
            PullObjects();
        }
        else
        {
            // Magnet býrakýlýnca Hold objesinden ayrýl
            holdTarget = null;
        }
    }

    void FixedUpdate()
    {
        // Maksimum dönme hýzýný sýnýrla
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
    }

    public float drag = 2f;
    void toZero()
    {
        rb.velocity = rb.velocity.x > 0 ? new Vector2(rb.velocity.x - drag * Time.deltaTime, rb.velocity.y) : new Vector2(rb.velocity.x + drag * Time.deltaTime, rb.velocity.y);
    }
    public void RollLeft()
    {
        rb.angularVelocity = rollSpeed;
    }

    public void RollRight()
    {
        rb.angularVelocity = -rollSpeed;
    }

    public void rideLeft()
    {
        Vector2 leftDir = -transform.right; // local -X yönü
        rb.AddForce(leftDir * moveSpeed * 3);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
    }

    public void rideRight()
    {
        Vector2 rightDir = transform.right; // local -X yönü
        rb.AddForce(rightDir * moveSpeed * 3);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
    }
    // --- Fly Sistemi ---
    public float flySpeed = 10f;
    public float flyDuration = 1.5f;   // max uçuþ süresi
    public float flyCooldown = 2.5f;   // tekrar uçabilmek için bekleme

    private bool isFlying = false;
    public bool canFly = true;
    private float flyTimer = 0f;
    private float cooldownTimer = 0f;
    void TryFly()
    {
        if (canFly)
        {
            if (!isFlying)
            {
                Debug.Log("sa");
                isFlying = true;
                flyTimer = flyDuration;
            }

            if (flyTimer > 0f)
            {
                Fly();
                flyTimer -= Time.deltaTime;
            }
            else
            {
                StopFly();
            }
        }
    }

    void Fly()
    {
        Debug.Log("as");
        Vector2 upDir = transform.up;
        rb.velocity += upDir * flySpeed * Time.deltaTime;
    }

    void StopFly()
    {
        isFlying = false;
        canFly = false;
        cooldownTimer = flyCooldown;
    }

    void LateUpdate()
    {
        // Cooldown sayacý
        if (!canFly)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canFly = true;
            }
        }
    }

    public GameObject ammo;
    public float projectileSpeed;
    public GameObject firstAmmo;
    public GameObject secondAmmo;
    public float magnetRange = 5f;
    public GameObject thirdAmmo;
    public bool canShoot;

    void shoot()
    {
        GameObject newAmmo = Instantiate(ammo);
        newAmmo.transform.position = gun.transform.position;
        newAmmo.transform.rotation = gun.transform.localRotation;
        newAmmo.GetComponent<Rigidbody2D>().velocity = gun.transform.right * projectileSpeed;
        Debug.Log(newAmmo.GetComponent<Rigidbody2D>().velocity);
        if (firstAmmo == null) firstAmmo = newAmmo;
        else if (secondAmmo == null) secondAmmo = newAmmo;
        else if (thirdAmmo == null) thirdAmmo = newAmmo;
        else
        {
            Destroy(firstAmmo);
            firstAmmo = secondAmmo;
            secondAmmo = thirdAmmo;
            thirdAmmo = newAmmo;
        }
    }

    public Vector2 boxSize = new Vector2(5f, 3f); // geniþlik, yükseklik
    public float forceAmount = 10f;               // kuvvet büyüklüðü 
    public Transform pullPoint;          // objeleri toplayacaðýn nokta (örneðin karakterin önü)
    public float magnetForce = 10f;
    public float holdPullForce = 20f; // hold objesine çekilme gücü
    private Transform holdTarget; // yapýþtýðýmýz obje
    private Vector3 holdOffset;


    void PullObjects()
    {
        // Kutunun merkezini ileri taþý
        Vector2 center = transform.position + transform.right * (boxSize.x / 2f);

        // Kutu taramasý
        Collider2D[] hits = Physics2D.OverlapBoxAll(center, boxSize, transform.eulerAngles.z);

        foreach (var hit in hits)
        {
            if (hit.attachedRigidbody == null) continue;

            // PULL
            if (hit.CompareTag("pull"))
            {
                Vector2 dir = (transform.position - hit.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * magnetForce);
            }

            // PUSH
            else if (hit.CompareTag("push"))
            {
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * magnetForce);
            }

            else if (hit.CompareTag("hold"))
            {
                holdTarget = hit.transform;
                Vector2 dir = (holdTarget.position - transform.position).normalized;
                rb.AddForce(dir * holdPullForce);
            }
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector2 center = (Vector2)transform.position + (Vector2)transform.right * magnetRange * 0.5f;
        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, transform.eulerAngles.z), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }
}
