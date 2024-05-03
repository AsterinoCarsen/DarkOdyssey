using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBoss : MonoBehaviour
{
    private Boss boss;
    private Animator transitionAnimator;
    private PlayerController playerScript;
    private GameObject player;
    private GameObject bossObject;
    private BossMusic bossMusic;

    void Start()
    {
        boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();
        transitionAnimator = GameObject.FindWithTag("Transition").GetComponent<Animator>();
        playerScript = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindWithTag("Player");
        bossObject = GameObject.FindWithTag("Boss");
        bossMusic = GameObject.FindWithTag("BGM").GetComponent<BossMusic>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(TriggerBridge());
    }

    private IEnumerator TriggerBridge()
    {
        playerScript.canMove = false;
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        transitionAnimator.SetTrigger("Flash");
        bossMusic.ChangeMusic();

        yield return new WaitForSeconds(2);

        bossObject.GetComponent<Animator>().SetTrigger("Open");

        yield return new WaitForSeconds(2);

        playerScript.canMove = true;
        boss.StartBoss();
        Destroy(gameObject.GetComponent<BoxCollider2D>());
    }
}
