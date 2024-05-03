using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4Chunk : MonoBehaviour
{

    private void Start()
    {
        Cursor.visible = true;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(0);
    }
}
