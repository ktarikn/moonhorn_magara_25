using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Kapý Ayarlarý")]
    [Tooltip("Kapýnýn açýkken ne kadar yukarý gideceðini belirler")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 3f, 0);

    [Tooltip("Kapýnýn açýlma/kapanma hýzý")]
    [SerializeField] private float moveSpeed = 3f;

    private Vector3 closedPosition; // Kapýnýn baþlangýçtaki (kapalý) pozisyonu
    private Vector3 targetPosition; // Kapýnýn gitmesi gereken hedef pozisyon

    void Start()
    {
        // Oyun baþladýðýnda kapýnýn mevcut pozisyonunu 'kapalý' olarak kaydet
        closedPosition = transform.position;
        // Baþlangýçta hedef, kapalý pozisyonun kendisidir
        targetPosition = closedPosition;
    }

    void Update()
    {
        // Kapýnýn pozisyonunu her frame'de hedefe doðru yumuþakça hareket ettir
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    // --- DIÞARIDAN KONTROL ÝÇÝN METOTLAR ---

    // Bu metot çaðrýldýðýnda kapý açýlmaya baþlayacak
    public void OpenDoor()
    {
        targetPosition = closedPosition + openOffset;
    }

    // Bu metot çaðrýldýðýnda kapý kapanmaya baþlayacak
    public void CloseDoor()
    {
        targetPosition = closedPosition;
    }
}