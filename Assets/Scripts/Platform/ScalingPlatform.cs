using UnityEngine;

public class ScalingPlatform : MonoBehaviour
{
    [SerializeField] private float startScaleY = 1f; // Ba�lang�� boyutu
    [SerializeField] private float endScaleY = 0.5f; // K���lece�i minimum boyut
    [SerializeField] private float speed = 1f; // H�z
    [SerializeField] private float waitTimeAtEnds = 1f; // U� noktalarda bekleme s�resi

    private bool shrinking = true; // K���l�yor mu b�y�yor mu?
    private float waitTimer = 0f;
    private Vector3 startScale;
    private Vector3 endScale;

    private void Start()
    {
        // Objeyi sadece Y ekseninde de�i�tiriyoruz
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
