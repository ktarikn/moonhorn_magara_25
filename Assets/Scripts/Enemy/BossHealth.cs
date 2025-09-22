using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    // Start is called before the first frame update
    public int boss_Health = 20;
    private bool isTakingDamage = false;

    [SerializeField] private SpriteRenderer sprite_1, sprite_2;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private Transform childTransform;

    [Header("level için")]
    [SerializeField] private int scene_int;

    void Start()
    {
        sprite_1 = GetComponent<SpriteRenderer>();
        sprite_1.enabled = false;
        sprite_2 = childTransform.GetComponent<SpriteRenderer>();
        sprite_2.enabled = false;
    }

    // Update is called once per frame

    public void getHit(int damage)
    {
        if (isTakingDamage) return;
        boss_Health -= damage;
        StartCoroutine(Damage());

        Debug.Log("boss can: " + boss_Health);

        if (boss_Health <= 0)
        {
            Debug.Log("uzun robot öldü");

            SceneManager.LoadScene(scene_int);
        }
    }
    void Update()
    {

    }

    private IEnumerator Damage()
    {
        isTakingDamage = true;

        sprite_1.enabled = true;
        sprite_2.enabled = true;

        yield return new WaitForSeconds(0.7f);

        sprite_1.enabled = false;
        sprite_2.enabled = false;

        yield return new WaitForSeconds(0.7f);

        sprite_1.enabled = true;
        sprite_2.enabled = true;

        yield return new WaitForSeconds(0.7f);

        sprite_1.enabled = false;
        sprite_2.enabled = false;

        isTakingDamage = false;
    }
}
