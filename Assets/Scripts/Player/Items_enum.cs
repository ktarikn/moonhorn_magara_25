using UnityEngine;

// Toplanabilir nesnelerin t�rlerini burada tan�ml�yoruz.
// Listeye yeni bir �ey eklemek isterseniz buraya eklemeniz yeterli.
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
    // Inspector'dan her bir objenin t�r�n� se�memizi sa�layacak de�i�ken
    public CollectibleType itemType;
}