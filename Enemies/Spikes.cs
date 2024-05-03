using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public float stunDuration;
    public Vector2 force;

    private PlayerAttack attackPlayer;
    private bool canDamage = true;

    private void Start()
    {
        attackPlayer = GameObject.FindWithTag("Player").GetComponent<PlayerAttack>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && attackPlayer.isSlicing == false && canDamage == true)
        {
            collision.GetComponent<PlayerController>().Damage(25, force, stunDuration);
            StartCoroutine(Grace());
        }
    }

    private IEnumerator Grace()
    {
        canDamage = false;

        yield return new WaitForSecondsRealtime(0.1f);

        canDamage = true;
    }
}
