using UnityEngine;

public class ScalingPlatform : MonoBehaviour
{
    [SerializeField] private float startScaleY = 1f; // Baþlangýç boyutu
    [SerializeField] private float endScaleY = 0.5f; // Küçüleceði minimum boyut
    [SerializeField] private float speed = 1f; // Hýz
    [SerializeField] private float waitTimeAtEnds = 1f; // Uç noktalarda bekleme süresi

    private bool shrinking = true; // Küçülüyor mu büyüyor mu?
    private float waitTimer = 0f;
    private Vector3 startScale;
    private Vector3 endScale;

    private void Start()
    {
        // Objeyi sadece Y ekseninde deðiþtiriyoruz
        startScale = new Vector3(transform.localScale.x, startScaleY, transform.localScale.z);
        endScale = new Vector3(transform.localScale.x, endScaleY, transform.localScale.z);

        transform.localScale = startScale;
    }

    private void Update()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        if (shrinking)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, endScale, speed * Time.deltaTime);

            if (Mathf.Approximately(transform.localScale.y, endScale.y))
            {
                shrinking = false;
                waitTimer = waitTimeAtEnds;
            }
        }
        else
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, startScale, speed * Time.deltaTime);

            if (Mathf.Approximately(transform.localScale.y, startScale.y))
            {
                shrinking = true;
                waitTimer = waitTimeAtEnds;
            }
        }
    }
}
