using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{

    public Text text;

    [HideInInspector]
    public bool isEnabled = true;

    private CanvasGroup canvas;
    private int mil;
    private int sec;
    private int min;
    private int hor;

    void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        StartCoroutine(AddWait());
    }

    void Update()
    {
        if (isEnabled == true)
        {
            canvas.alpha = 1;
        } else if (isEnabled == false)
        {
            canvas.alpha = 0;
        }

        text.text = hor + "h:" + min + "m:" + sec + "s." + mil;
    }

    private IEnumerator AddWait()
    {
        if (mil + 1 == 10)
        {
            mil = 0;
            sec++;
        }
        else if (sec + 1 == 60)
        {
            sec = 0;
            min++;
        }
        else if (min + 1 == 60)
        {
            min = 0;
            hor++;
        }
        else
        {
            mil++;
        }

        yield return new WaitForSecondsRealtime(0.1f);
        StartCoroutine(AddWait());
    }
}
