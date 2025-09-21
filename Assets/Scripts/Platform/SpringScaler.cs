using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpringScaler : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private float startScaleY = 1f;
    [SerializeField] private float endScaleY = 0.4f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float waitTimeAtEnds = 0.3f;

    [Header("Top follower (platform)")]
    [Tooltip("Spring ile ayn� parent alt�nda, sibling olacak �ekilde atay�n (TopPlatform).")]
    [SerializeField] private Transform topFollower; // TopPlatform referans�

    // internals
    private float initialSpringLocalY;
    private float initialTopLocalY;
    private float initialTopOffsetLocal;
    private float initialScaleY;

    private bool shrinking = true;
    private float waitTimer = 0f;

    private void Start()
    {
        // Ba�lang�� boyutunu zorla uygula
        Vector3 s = transform.localScale;
        s.y = startScaleY;
        transform.localScale = s;

        initialScaleY = transform.localScale.y;

        if (topFollower != null)
        {
            if (transform.parent == null || topFollower.parent != transform.parent)
            {
                Debug.LogWarning("SpringScaler: topFollower'�n ayn� parent alt�nda (sibling) olmas�na dikkat et. Bu davran�� daha stabil �al���r.");
            }

            initialSpringLocalY = transform.localPosition.y;
            initialTopLocalY = topFollower.localPosition.y;
            initialTopOffsetLocal = initialTopLocalY - initialSpringLocalY;
        }
    }

    private void Update()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        float current = transform.localScale.y;
        float target = shrinking ? endScaleY : startScaleY;
        float next = Mathf.MoveTowards(current, target, speed * Time.deltaTime);

        Vector3 s = transform.localScale;
        s.y = next;
        transform.localScale = s;

        if (Mathf.Approximately(next, target))
        {
            shrinking = !shrinking;
            waitTimer = waitTimeAtEnds;
        }
    }

    private void LateUpdate()
    {
        // Spring'in scale'�na g�re top platform'un local Y'sini g�ncelle
        if (topFollower == null) return;

        // oran = newScale / startScale
        float scaleRatio = transform.localScale.y / initialScaleY;
        float newTopLocalY = initialSpringLocalY + initialTopOffsetLocal * scaleRatio;

        Vector3 loc = topFollower.localPosition;
        loc.y = newTopLocalY;
        topFollower.localPosition = loc;
    }

    private void OnValidate()
    {
        // safety clamps
        if (endScaleY < 0.01f) endScaleY = 0.01f;
        if (startScaleY < endScaleY) startScaleY = endScaleY;
        if (speed < 0f) speed = 0f;
        if (waitTimeAtEnds < 0f) waitTimeAtEnds = 0f;
    }
}
