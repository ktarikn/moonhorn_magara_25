using UnityEngine;

public class Wheel : MonoBehaviour
{
    // Tekerleðin dönüþ hýzýný konveyör hýzýyla eþleþtirmek için bir çarpan.
    // Görsel olarak doðru hýzý bulmak için Inspector'dan bu deðeri ayarlayabilirsiniz.
    [SerializeField] private float rotationMultiplier = 50f;

    // Bu metodu Conveyor script'i çaðýracak.
    public void Rotate(float conveyorSpeed, bool isMovingRight)
    {
        // Saða gidiyorsa saat yönünde (-1), sola gidiyorsa saat yönünün tersine (1) dönecek.
        float direction = isMovingRight ? -1f : 1f;

        // Dönüþ miktarýný hesapla: hýz * yön * zaman
        float rotationAmount = conveyorSpeed * rotationMultiplier * direction * Time.deltaTime;

        // Tekerleði Z ekseninde döndür.
        transform.Rotate(0f, 0f, rotationAmount);
    }
}