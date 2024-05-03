using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicators : MonoBehaviour
{
    public Slider healthBar;

    public Slider attack1;
    public Slider attack2;
    public Slider attack3;

    private PlayerController controller;

    private void Start()
    {
        controller = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        healthBar.maxValue = controller.maxHealth;
    }

    private void LateUpdate()
    {
        healthBar.value = Mathf.Lerp(healthBar.value, controller.maxHealth, 0.01f);
    }

    public void StartCD(int type, float coolDown)
    {
        if (type == 1)
        {
            StartCoroutine(Attack1(coolDown));
        } else if (type == 2)
        {
            StartCoroutine(Attack2(coolDown));
        } else if (type == 3)
        {
            StartCoroutine(Attack3(coolDown));
        }
    }

    private IEnumerator Attack1(float duration)
    {
        attack1.maxValue = duration;
        attack1.value = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            attack1.value += Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Attack2(float duration)
    {
        attack2.maxValue = duration;
        attack2.value = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            attack2.value += Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Attack3(float duration)
    {
        attack3.maxValue = duration;
        attack3.value = 0;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            attack3.value += Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
