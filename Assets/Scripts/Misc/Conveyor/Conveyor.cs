using Unity.VisualScripting;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    [Header("Conveyor Settings")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool isRight;

    [Header("Components")]
    [SerializeField] private Wheel[] wheels;

    [SerializeField] private SpriteRenderer conveyorSpriteRenderer;

    private void Update()
    {
        foreach (Wheel wheel in wheels)
            wheel.Rotate(speed, isRight);

        if (conveyorSpriteRenderer is not null)
            conveyorSpriteRenderer.flipY = !isRight;

    }




    // Üzerinde duran nesneleri hareket ettir
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Hareket vektörünü hesapla (Bu kýsým ayný kalýyor)
            Vector3 move = speed * Time.deltaTime * (isRight ? Vector3.right : Vector3.left);

            // DÜZELTME: Hareketi objenin kendi yönüne göre deðil,
            // dünyanýn yönüne göre ("Space.World") uygula.
            collision.transform.Translate(move, Space.World);
        }
    }
}