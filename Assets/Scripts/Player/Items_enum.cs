using UnityEngine;

// Toplanabilir nesnelerin türlerini burada tanýmlýyoruz.
// Listeye yeni bir þey eklemek isterseniz buraya eklemeniz yeterli.
public enum CollectibleType
{
    Eye,
    Wheel,
    Magnet,
    Weapon,
    Drone
}

public class Items_enum : MonoBehaviour
{
    // Inspector'dan her bir objenin türünü seçmemizi saðlayacak deðiþken
    public CollectibleType itemType;
}