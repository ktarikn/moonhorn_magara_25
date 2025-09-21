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




    // �zerinde duran nesneleri hareket ettir
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Hareket vekt�r�n� hesapla (Bu k�s�m ayn� kal�yor)
            Vector3 move = speed * Time.deltaTime * (isRight ? Vector3.right : Vector3.left);

            // D�ZELTME: Hareketi objenin kendi y�n�ne g�re de�il,
            // d�nyan�n y�n�ne g�re ("Space.World") uygula.
            collision.transform.Translate(move, Space.World);
        }
    }
}