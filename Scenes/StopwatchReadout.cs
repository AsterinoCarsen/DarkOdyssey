using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchReadout : MonoBehaviour
{
    public Text text;

    private int mil;
    private int sec;
    private int min;
    private int hor;

    void Start()
    {
        mil = PlayerPrefs.GetInt("mil", 0);
        sec = PlayerPrefs.GetInt("sec", 0);
        min = PlayerPrefs.GetInt("min", 0);
        hor = PlayerPrefs.GetInt("hor", 0);

        text.text = hor + "h:" + min + "m:" + sec + "s." + mil;
    }
}
