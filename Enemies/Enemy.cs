using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public bool isImmuneToFall;
    public string enemyType;
    public AudioClip hit;
    public AudioClip die;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 startPos;
    private SpriteRenderer spr;
    private Transform player;
    private Speaker speaker;

    private bool playOnce = true;
    private bool canAnimate = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        speaker = GameObject.FindWithTag("Speaker").GetComponent<Speaker>();
        player = GameObject.FindWithTag("Player").transform;
        spr = GetComponent<SpriteRenderer>();

        startPos = transform.position;
    }

    private void Update()
    {
        if (canAnimate == true)
        {
            anim.SetFloat("xSpeed", Mathf.Abs(rb.velocity.x));

            // Flip sprite
            if ((player.position.x - transform.position.x) < 0.4f && playOnce == true)
            {
                spr.flipX = true;
            }
            else if ((player.position.x - transform.position.x) > -0.4f && playOnce == true)
            {
                spr.flipX = false;
            }
        }

        if (isImmuneToFall == true && transform.position.y < -20)
        {
            transform.position = startPos;
        }
    }


    public void Damage(float damage, float stunDuration, Vector2 force)
    {
        StartCoroutine(DamageBridge(damage, stunDuration, force));
    }

    private IEnumerator DamageBridge(float damage, float stunDuration, Vector2 force)
    {
        maxHealth -= damage;

        if (maxHealth <= 0 && playOnce == true)
        {
            playOnce = false;
            anim.StopPlayback();
            anim.SetTrigger("Die");
            speaker.PlaySound(die, 0.75f, false);
            canAnimate = false;

            if (enemyType == "Dog")
            {
                Destroy(GetComponent<Dog>());
            }
            else if (enemyType == "Guard")
            {
                Destroy(GetComponent<Guard>());
            }
            else if (enemyType == "Rock")
            {
                Destroy(GetComponent<Rock>());
            }
            else if (enemyType == "Mage")
            {
                Destroy(GetComponent<Mage>());
            }
            else if (enemyType == "Electric")
            {
                Destroy(GetComponent<Electric>());
            }
            else if (enemyType == "Boss")
            {
                GetComponent<Boss>().Die();
            }

            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.drag = 1000;

            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);

            Destroy(gameObject);
        }
        else if (playOnce == true)
        {
            anim.SetTrigger("Hurt");
            speaker.PlaySound(hit, 0.75f, true);

            if (enemyType == "Dog")
            {
                GetComponent<Dog>().enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = force;

                yield return new WaitForSeconds(stunDuration);

                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Dog>().enabled = true;
            }
            else if (enemyType == "Guard")
            {
                GetComponent<Guard>().enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = force;

                yield return new WaitForSeconds(stunDuration);

                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Guard>().enabled = true;
            }
            else if (enemyType == "Rock")
            {
                GetComponent<Rock>().enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = force;

                yield return new WaitForSeconds(stunDuration);

                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Rock>().enabled = true;
            }
            else if (enemyType == "Mage")
            {
                GetComponent<Mage>().enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                //rb.velocity = force;

                yield return new WaitForSeconds(stunDuration);

                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Mage>().enabled = true;
            }
            else if (enemyType == "Electric")
            {
                GetComponent<Electric>().enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = force;

                yield return new WaitForSeconds(stunDuration);

                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Electric>().enabled = true;
            } else if (enemyType == "Boss")
            {
                GetComponent<Boss>().enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezePositionX;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                rb.velocity = force;

                yield return new WaitForSeconds(stunDuration);

                rb.constraints = RigidbodyConstraints2D.None;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                GetComponent<Boss>().enabled = true;
            }

            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }
}
