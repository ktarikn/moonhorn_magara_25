using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Kap� Ayarlar�")]
    [Tooltip("Kap�n�n a��kken ne kadar yukar� gidece�ini belirler")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3f, 0);

    [Tooltip("Kap�n�n a��lma/kapanma h�z�")]
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 closedPosition; // Kap�n�n ba�lang��taki (kapal�) pozisyonu
    private Vector3 targetPosition; // Kap�n�n gitmesi gereken hedef pozisyon

    void Start()
    {
        // Oyun ba�lad���nda kap�n�n mevcut pozisyonunu 'kapal�' olarak kaydet
        closedPosition = transform.position;
        // Ba�lang��ta hedef, kapal� pozisyonun kendisidir
        targetPosition = closedPosition;
    }

    void Update()
    {
        // Kap�n�n pozisyonunu her frame'de hedefe do�ru yumu�ak�a hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // --- DI�ARIDAN KONTROL ���N METOTLAR ---

    // Bu metot �a�r�ld���nda kap� a��lmaya ba�layacak
    public void OpenDoor()
    {
        targetPosition = closedPosition + openOffset;
    }

    // Bu metot �a�r�ld���nda kap� kapanmaya ba�layacak
    public void CloseDoor()
    {
        targetPosition = closedPosition;
    }
}