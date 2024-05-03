using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Slider volumeSlider;
    public Toggle toggle;

    private AudioSource speaker;
    private Stopwatch stopWatch;
    private Canvas menu;
    private NewScene newScene;

    private bool isPaused = false;

    private void Awake()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.5f);

        if (PlayerPrefs.GetInt("stopwatch", 0) == 0)
        {
            toggle.isOn = false;
        } else if (PlayerPrefs.GetInt("stopwatch", 0) == 1)
        {
            toggle.isOn = true;
        }
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetFloat("volume", volumeSlider.value);

        if (toggle.isOn == true)
        {
            PlayerPrefs.SetInt("stopwatch", 1);
        } else if (toggle.isOn == false)
        {
            PlayerPrefs.SetInt("stopwatch", 0);
        }
    }

    void Start()
    {
        speaker = GameObject.FindWithTag("BGM").GetComponent<AudioSource>();
        stopWatch = GameObject.FindWithTag("StopWatch").GetComponent<Stopwatch>();
        newScene = GameObject.FindWithTag("Transition").GetComponent<NewScene>();
        menu = GetComponent<Canvas>();

        speaker.volume = volumeSlider.value;
    }

    void Update()
    {
        speaker.volume = volumeSlider.value;
        stopWatch.isEnabled = toggle.isOn;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
        }

        if (isPaused == true)
        {
            menu.enabled = true;
            Time.timeScale = 0;
            volumeSlider.interactable = true;
            toggle.interactable = true;
            Cursor.visible = true;
        } else if (isPaused == false)
        {
            menu.enabled = false;
            Time.timeScale = 1;
            volumeSlider.interactable = false;
            toggle.interactable = false;
            Cursor.visible = false;
        }
    }

    public void LoadFromCheckpoint()
    {
        newScene.ReloadCurrentScene();
        isPaused = false;
    }
}
