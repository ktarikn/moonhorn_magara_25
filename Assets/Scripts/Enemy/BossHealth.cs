using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int lives = 3;
    private bool isTakingDamage = false;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    // Update is called once per frame

    public void getHit()
    {
        if (isTakingDamage) return;
        lives--;
        StartCoroutine(Damage());

        if (lives <= 0)
        {

            //level_end()
        }
    }
    void Update()
    {
        
    }

    private IEnumerator Damage()
    {
        isTakingDamage = true;

        spriteRenderer.enabled = true;

        yield return new WaitForSeconds(0.7f);

        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(0.7f);

        spriteRenderer.enabled = true;

        yield return new WaitForSeconds(0.7f);

        spriteRenderer.enabled = false;

        isTakingDamage = false;
    }
}
