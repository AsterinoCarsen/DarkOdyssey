using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private NewScene sceneChanger;

    void Start()
    {
        sceneChanger = GameObject.FindWithTag("Transition").GetComponent<NewScene>();
    }

    public void LoadGame()
    {
        sceneChanger.ChangeScene(1);
    }
}
