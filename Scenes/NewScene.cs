using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewScene : MonoBehaviour
{
    private Image panel;
    private Animator anim;

    private bool playOnce = true;

    private void Start()
    {
        panel = GetComponent<Image>();
        anim = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().buildIndex == 3 || SceneManager.GetActiveScene().buildIndex == 4)
        {
            PlayerPrefs.SetInt("Checkpoint", 3);
        } else if (SceneManager.GetActiveScene().buildIndex == 5 || SceneManager.GetActiveScene().buildIndex == 6)
        {
            PlayerPrefs.SetInt("Checkpoint", 5);
        } else if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            PlayerPrefs.SetInt("Checkpoint", 7);
        }
        else
        {
            PlayerPrefs.SetInt("Checkpoint", 1);
        }
    }

    public void ChangeScene(int buildIndex)
    {
        if (playOnce == true)
        {
            StartCoroutine(ChangeSceneBridge(buildIndex));
        }
        playOnce = false;
    }

    public void ReloadCurrentScene()
    {
        StartCoroutine(ChangeSceneBridge(SceneManager.GetActiveScene().buildIndex));
    }

    public void ChangeSceneDelay(int buildIndex, float delay)
    {
        StartCoroutine(changeSceneDelayBridge(buildIndex, delay));
    }

    private IEnumerator changeSceneDelayBridge(int buildIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        ChangeScene(buildIndex);
    }

    public void LoadLastCheckpoint()
    {
        ChangeScene(PlayerPrefs.GetInt("Checkpoint"));
    }

    private IEnumerator ChangeSceneBridge(int buildIndex)
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(buildIndex);
    }
}
