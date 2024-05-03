using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    private int enemiesLeft;
    private int playOnce = 0;

    private NewScene sceneChanger;
    private Animator anim;

    private void Start()
    {
        sceneChanger = GameObject.FindWithTag("Transition").GetComponent<NewScene>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        enemiesLeft = GameObject.FindGameObjectsWithTag("Guard").Length + GameObject.FindGameObjectsWithTag("Dog").Length + GameObject.FindGameObjectsWithTag("Electric").Length + GameObject.FindGameObjectsWithTag("Mage").Length + GameObject.FindGameObjectsWithTag("Rock").Length;

        if (enemiesLeft == 0 && playOnce == 0)
        {
            anim.SetTrigger("Open");
            playOnce++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && enemiesLeft == 0)
        {
            Destroy(collision.GetComponent<Rigidbody2D>());
            sceneChanger.ChangeScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
