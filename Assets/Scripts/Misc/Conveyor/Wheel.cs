using UnityEngine;

public class Wheel : MonoBehaviour
{
    // Tekerle�in d�n�� h�z�n� konvey�r h�z�yla e�le�tirmek i�in bir �arpan.
    // G�rsel olarak do�ru h�z� bulmak i�in Inspector'dan bu de�eri ayarlayabilirsiniz.
    [SerializeField] private float rotationMultiplier = 50f;

    // Bu metodu Conveyor script'i �a��racak.
    public void Rotate(float conveyorSpeed, bool isMovingRight)
    {
        // Sa�a gidiyorsa saat y�n�nde (-1), sola gidiyorsa saat y�n�n�n tersine (1) d�necek.
        float direction = isMovingRight ? -1f : 1f;

        // D�n�� miktar�n� hesapla: h�z * y�n * zaman
        float rotationAmount = conveyorSpeed * rotationMultiplier * direction * Time.deltaTime;

        // Tekerle�i Z ekseninde d�nd�r.
        transform.Rotate(0f, 0f, rotationAmount);
    }
}