using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    [System.Serializable]  //chatgptden caldým kodu
    public class Layer
    {
        public Transform layerTransform;
        [Tooltip("0 = sabit (arka planda hiç hareket etmez), 1 = kamera ile ayný hýz")]
        public Vector2 parallaxMultiplier = new Vector2(0.5f, 0f);
        [Tooltip("Sprite yatay olarak tekrar ediyorsa true yap")]
        public bool repeat = false;

        // runtime
        [HideInInspector] public Vector3 startPos;
        [HideInInspector] public float spriteWidth = 0f;
        [HideInInspector] public float spriteHeight = 0f;
    }

    public Layer[] layers;
    Camera cam;
    Vector3 camStartPos;

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("ParallaxBackground: Main Camera bulunamadý.");
            enabled = false;
            return;
        }

        camStartPos = cam.transform.position;

        foreach (var l in layers)
        {
            if (l.layerTransform == null) continue;
            l.startPos = l.layerTransform.position;

            if (l.repeat)
            {
                // SpriteRenderer veya Renderer üzerinden sprite boyutunu al
                var sr = l.layerTransform.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    l.spriteWidth = sr.bounds.size.x;
                    l.spriteHeight = sr.bounds.size.y;
                }
                else
                {
                    var r = l.layerTransform.GetComponent<Renderer>();
                    if (r != null)
                    {
                        l.spriteWidth = r.bounds.size.x;
                        l.spriteHeight = r.bounds.size.y;
                    }
                    else
                    {
                        Debug.LogWarning("ParallaxBackground: repeat açýk ama SpriteRenderer/Renderer bulunamadý on " + l.layerTransform.name);
                        l.repeat = false;
                    }
                }
            }
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 camDeltaFromStart = cam.transform.position - camStartPos;

        foreach (var l in layers)
        {
            if (l.layerTransform == null) continue;

            // Temel parallax: baþlangýç konumuna göre kameranýn hareketine oranla konum hesapla
            float newX = l.startPos.x + camDeltaFromStart.x * l.parallaxMultiplier.x;
            float newY = l.startPos.y + camDeltaFromStart.y * l.parallaxMultiplier.y;
            l.layerTransform.position = new Vector3(newX, newY, l.layerTransform.position.z);

            // Eðer tekrarlama açýksa yatayda (X) tekrar ettir
            if (l.repeat && l.spriteWidth > 0f)
            {
                // Kamera ile layer arasýndaki fark
                float diff = cam.transform.position.x - l.layerTransform.position.x;
                // Eðer fark spriteWidth'den büyükse, layer.startPos'ý kaydýr
                if (Mathf.Abs(diff) >= l.spriteWidth)
                {
                    float shift = Mathf.Sign(diff) * l.spriteWidth;
                    l.startPos.x += shift;
                    // hemen pozisyonu güncelle (yeni startPos ile)
                    l.layerTransform.position = new Vector3(l.startPos.x + camDeltaFromStart.x * l.parallaxMultiplier.x,
                                                            l.layerTransform.position.y,
                                                            l.layerTransform.position.z);
                }
            }
        }
    }

    // Inspector'da deðiþiklik yapýldýðýnda baþlangýç deðerlerini güncellemek iyi olur
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            cam = Camera.main;
            if (cam != null)
                camStartPos = cam.transform.position;
            foreach (var l in layers) if (l.layerTransform != null) l.startPos = l.layerTransform.position;
        }
    }
}
