using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public GameObject healthBar;
    public Vector2 offset;

    private GameObject bar;
    private Slider slide;
    private Animator anim;

    void Start()
    {
        bar = Instantiate(healthBar, transform.position + new Vector3(0, 1), Quaternion.identity, GameObject.FindWithTag("WorldSpace").transform);
        slide = bar.GetComponent<Slider>();
        anim = GetComponent<Animator>();

        slide.maxValue = transform.GetComponent<Enemy>().maxHealth;
    }

    void LateUpdate()
    {
        if (transform.GetComponent<Enemy>().maxHealth <= 0)
        {
            Destroy(bar, anim.GetCurrentAnimatorClipInfo(0).Length);
        }

        bar.transform.position = (Vector2)transform.position + offset;
        slide.value = Mathf.Lerp(slide.value, transform.GetComponent<Enemy>().maxHealth, 0.01f);
    }
}
