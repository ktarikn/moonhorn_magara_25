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
    public float rollSpeed = 200f; // torque g�c�
    public float maxAngularSpeed = 300f; // max d�nme h�z� (derece/sn)

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
            TryFly(); // u�may� deneyelim
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
        if (Input.GetMouseButton(1)) // m�knat�s aktif
        {
            if(canMagnet) PullObjects();
        }
        else
        {
            // Magnet b�rak�l�nca Hold objesinden ayr�l
            holdTarget = null;
        }
    }

    void FixedUpdate()
    {
        // Maksimum d�nme h�z�n� s�n�rla
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
        Vector2 leftDir = -transform.right; // local -X y�n�
        rb.AddForce(leftDir * moveSpeed * 3);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
    }

    public void rideRight()
    {
        Vector2 rightDir = transform.right; // local -X y�n�
        rb.AddForce(rightDir * moveSpeed * 3);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed), rb.velocity.y);
    }
    // --- Fly Sistemi ---
    public float flySpeed = 10f;
    public float flyDuration = 1.5f;   // max u�u� s�resi
    public float flyCooldown = 2.5f;   // tekrar u�abilmek i�in bekleme

    public bool hasHeli = false;
    private bool isFlying = false;
    public bool canFly = true;
    private float flyTimer = 0f;
    private float cooldownTimer = 0f;
    void TryFly()
    {
        if (canFly && hasHeli)
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
        // Cooldown sayac�
        if (!canFly)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canFly = true;
            }
        }
    }

    public GameObject bulletPrefab;
    public float shootForce;
    public GameObject firstAmmo;
    public GameObject secondAmmo;
    
    public GameObject thirdAmmo;
    public bool canShoot;
    public Transform firePoint;     // merminin ��kaca�� nokta
    void shoot()
    {

        GameObject newAmmo = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Mermiye Rigidbody2D ekleyip kuvvet uygula
        Rigidbody2D a_rb = newAmmo.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            a_rb.gravityScale = 1f; // yer�ekimi etkisi
            a_rb.velocity = gun.transform.right * shootForce; // karakterin sa� y�n�
        }


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
    public float magnetRange = 5f;
    public Vector2 boxSize = new Vector2(5f, 3f); // geni�lik, y�kseklik
    public float forceAmount = 10f;               // kuvvet b�y�kl��� 
    public Transform pullPoint;          // objeleri toplayaca��n nokta (�rne�in karakterin �n�)
    public float magnetForce = 10f;
    public float holdPullForce = 20f; // hold objesine �ekilme g�c�
    private Transform holdTarget; // yap��t���m�z obje
    private Vector3 holdOffset;
    public bool canMagnet = false;
    public GameObject magnetHead;
    void PullObjects()
    {
        // Kutunun merkezini ileri ta��
        Vector2 center = transform.position + transform.right * (boxSize.x / 2f);

        // Kutu taramas�
        Collider2D[] hits = Physics2D.OverlapBoxAll(magnetHead.transform.position, boxSize, magnetHead.transform.eulerAngles.z);

        foreach (var hit in hits)
        {
            if (hit.attachedRigidbody == null) continue;

            // PULL
            if (hit.CompareTag("pull"))
            {
                Vector2 dir = (magnetHead.transform.position - hit.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * magnetForce);
            }

            // PUSH
            else if (hit.CompareTag("push"))
            {
                Vector2 dir = (hit.transform.position - magnetHead.transform.position).normalized;
                hit.attachedRigidbody.AddForce(dir * magnetForce);
            }

            else if (hit.CompareTag("hold"))
            {
                holdTarget = hit.transform;
                Vector2 dir = (holdTarget.position - magnetHead.transform.position).normalized;
                rb.AddForce(dir * holdPullForce);
            }
        }
    }


    void OnDrawGizmos()
    {
        if (magnetHead == null) return;

        // Gizmo rengi
        Gizmos.color = Color.cyan;

        // OverlapBox pozisyonu ve boyutu
        Vector2 pos = magnetHead.transform.position;
        Vector2 size = boxSize;
        float angle = magnetHead.transform.eulerAngles.z;

        // Unity 2D i�in Rotate ve DrawWireCube kombinasyonu
        Gizmos.matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0, 0, angle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, size);

        // Matrisi s�f�rla ki ba�ka �izimler etkilenmesin
        Gizmos.matrix = Matrix4x4.identity;
    }
}
