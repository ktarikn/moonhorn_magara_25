using System;
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            keyAAction?.Invoke();
        }
        if (Input.GetKey(KeyCode.D))
        {
            keyDAction?.Invoke();
        }
        if (Input.GetKey(KeyCode.W))
        {
            keyWAction?.Invoke();
        }
        if (Input.GetKey(KeyCode.S))
        {
            keySAction?.Invoke();
        }
    }

    void FixedUpdate()
    {
        // Maksimum d�nme h�z�n� s�n�rla
        rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularSpeed, maxAngularSpeed);
    }

    public void RollLeft()
    {
        rb.angularVelocity = 200f;
    }

    public void RollRight()
    {
        rb.angularVelocity = -200f;
    }
}
