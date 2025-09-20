using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    [System.Serializable]  //chatgptden cald�m kodu
    public class Layer
    {
        public Transform layerTransform;
        [Tooltip("0 = sabit (arka planda hi� hareket etmez), 1 = kamera ile ayn� h�z")]
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
            Debug.LogError("ParallaxBackground: Main Camera bulunamad�.");
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
                // SpriteRenderer veya Renderer �zerinden sprite boyutunu al
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
                        Debug.LogWarning("ParallaxBackground: repeat a��k ama SpriteRenderer/Renderer bulunamad� on " + l.layerTransform.name);
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

            // Temel parallax: ba�lang�� konumuna g�re kameran�n hareketine oranla konum hesapla
            float newX = l.startPos.x + camDeltaFromStart.x * l.parallaxMultiplier.x;
            float newY = l.startPos.y + camDeltaFromStart.y * l.parallaxMultiplier.y;
            l.layerTransform.position = new Vector3(newX, newY, l.layerTransform.position.z);

            // E�er tekrarlama a��ksa yatayda (X) tekrar ettir
            if (l.repeat && l.spriteWidth > 0f)
            {
                // Kamera ile layer aras�ndaki fark
                float diff = cam.transform.position.x - l.layerTransform.position.x;
                // E�er fark spriteWidth'den b�y�kse, layer.startPos'� kayd�r
                if (Mathf.Abs(diff) >= l.spriteWidth)
                {
                    float shift = Mathf.Sign(diff) * l.spriteWidth;
                    l.startPos.x += shift;
                    // hemen pozisyonu g�ncelle (yeni startPos ile)
                    l.layerTransform.position = new Vector3(l.startPos.x + camDeltaFromStart.x * l.parallaxMultiplier.x,
                                                            l.layerTransform.position.y,
                                                            l.layerTransform.position.z);
                }
            }
        }
    }

    // Inspector'da de�i�iklik yap�ld���nda ba�lang�� de�erlerini g�ncellemek iyi olur
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
