// MovingPlatform.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Yerel eksende hareket y�n�. (1,0)=sa�a, (0,1)=yukar�")]
    [SerializeField] private Vector2 localDirection = Vector2.right;
    [Tooltip("Hareket mesafesi (metre).")]
    [SerializeField] private float distance = 5f;
    [Tooltip("H�z (m/s).")]
    [SerializeField] private float speed = 2f;
    [Tooltip("Yerel (transform y�n�ne g�re) mi yoksa d�nya eksenine g�re mi hareket etsin?")]
    [SerializeField] private bool useLocalSpace = true;
    [Tooltip("Hedefe var�nca bekleme s�resi (saniye).")]
    [SerializeField] private float waitAtEnds = 0.3f;
    [Tooltip("Ba�lang��ta ters y�nden ba�las�n m�? (hedefe do�ru ba�larsa false)")]
    [SerializeField] private bool startReversed = false;

    // runtime
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 targetPos;
    private bool movingToTarget;
    private float waitTimer = 0f;

    // player parenting i�in �nceki parent saklamas� (birden fazla nesne olabilir)
    private Dictionary<Transform, Transform> originalParents = new Dictionary<Transform, Transform>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Kinematic olmas�n� zorla � jam ortam�nda beklenmedik ayarlar� �nlemek i�in.
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true; // physics sim�lasyonunda olsun
    }

    private void Start()
    {
        startPos = rb.position;

        Vector2 dir = localDirection.normalized;
        if (useLocalSpace)
            dir = (Vector2)transform.TransformDirection(dir);

        targetPos = startPos + dir * distance;
        movingToTarget = !startReversed;
    }

    private void FixedUpdate()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector2 current = rb.position;
        Vector2 target = movingToTarget ? targetPos : startPos;

        // MoveTowards ile sabit h�zda hareket
        Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        // Hedefe ula�t�k m�?
        if ((next - target).sqrMagnitude < 0.0001f)
        {
            // y�n de�i�tir
            movingToTarget = !movingToTarget;
            waitTimer = waitAtEnds;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 sPos = transform.position;
        Vector3 dir = useLocalSpace ? transform.TransformDirection(localDirection.normalized) : (Vector3)localDirection.normalized;
        Vector3 tPos = sPos + dir * distance;
        Gizmos.DrawLine(sPos, tPos);
        Gizmos.DrawSphere(sPos, 0.08f);
        Gizmos.DrawSphere(tPos, 0.08f);
    }
}