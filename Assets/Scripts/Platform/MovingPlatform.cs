// MovingPlatform.cs
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("Yerel eksende hareket yönü. (1,0)=saða, (0,1)=yukarý")]
    [SerializeField] private Vector2 localDirection = Vector2.right;
    [Tooltip("Hareket mesafesi (metre).")]
    [SerializeField] private float distance = 5f;
    [Tooltip("Hýz (m/s).")]
    [SerializeField] private float speed = 2f;
    [Tooltip("Yerel (transform yönüne göre) mi yoksa dünya eksenine göre mi hareket etsin?")]
    [SerializeField] private bool useLocalSpace = true;
    [Tooltip("Hedefe varýnca bekleme süresi (saniye).")]
    [SerializeField] private float waitAtEnds = 0.3f;
    [Tooltip("Baþlangýçta ters yönden baþlasýn mý? (hedefe doðru baþlarsa false)")]
    [SerializeField] private bool startReversed = false;

    // runtime
    private Rigidbody2D rb;
    private Vector2 startPos;
    private Vector2 targetPos;
    private bool movingToTarget;
    private float waitTimer = 0f;

    // player parenting için önceki parent saklamasý (birden fazla nesne olabilir)
    private Dictionary<Transform, Transform> originalParents = new Dictionary<Transform, Transform>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Kinematic olmasýný zorla — jam ortamýnda beklenmedik ayarlarý önlemek için.
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true; // physics simülasyonunda olsun
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

        // MoveTowards ile sabit hýzda hareket
        Vector2 next = Vector2.MoveTowards(current, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(next);

        // Hedefe ulaþtýk mý?
        if ((next - target).sqrMagnitude < 0.0001f)
        {
            // yön deðiþtir
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