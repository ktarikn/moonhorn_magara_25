using System;
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


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if(canCarMove) isGrounded = Physics2D.OverlapCircle(carGroundCheck1.position, checkRadius, groundLayer) || Physics2D.OverlapCircle(carGroundCheck2.position, checkRadius, groundLayer);
        if (Input.GetKey(KeyCode.A))
        {
            if (isGrounded) rideLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (isGrounded) rideRight();
        }
        if (Input.GetKey(KeyCode.W))
        {
            TryFly(); // uçmayý deneyelim
        }
        if (Input.GetKey(KeyCode.S))
        {
            keySAction?.Invoke();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            RollLeft();
        }
        if (Input.GetKey(KeyCode.E))
        {
            RollRight();
        }
    }

    void FixedUpdate()
    {
        // Maksimum dönme hýzýný sýnýrla
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
    }

    public void RollLeft()
    {
        rb.angularVelocity = rollSpeed;
    }

    public void RollRight()
    {
        rb.angularVelocity = -rollSpeed;
    }

    public float moveForce = 50f;
    public void rideLeft()
    {
        Vector2 leftDir = -transform.right; // local -X yönü
        rb.velocity = leftDir * moveSpeed;
    }

    public void rideRight()
    {
        Vector2 rightDir = transform.right; // local -X yönü
        rb.velocity = rightDir * moveSpeed;
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
        rb.velocity = upDir * flySpeed;
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
}
