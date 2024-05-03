using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    [Header("Movement")]
    public float alertDistance;
    public float moveSpeed;

    [Header("Attack")]
    public float attackDamage;
    public float stunDuration;
    public Vector2 knockBack;

    [Header("Audio")]
    public AudioClip attack;

    [HideInInspector]
    public bool canMove = true;

    private Animator anim;
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Speaker speaker;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;
        speaker = GameObject.FindWithTag("Speaker").GetComponent<Speaker>();
    }

    private void Update()
    {
        if (canMove == false)
        {
            rb.velocity = new Vector2(0, 0);
        }

        if (Vector2.Distance(player.position, transform.position) < alertDistance && canMove == true)
        {
            if ((transform.position.x - player.position.x) > 0)
            {
                // Left
                if (Vector2.Distance(player.position, transform.position) < 4)
                {
                    Attack();
                }
                else
                {
                    RaycastHit2D rayInfo = Physics2D.Raycast(transform.position - new Vector3(2, 0), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
                    if (rayInfo.transform == null)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                    }
                }
            }
            else if ((transform.position.x - player.position.x) < 0)
            {
                // Right
                if (Vector2.Distance(player.position, transform.position) < 4)
                {
                    Attack();
                }
                else
                {
                    RaycastHit2D rayInfo = Physics2D.Raycast(transform.position + new Vector3(2, 0), Vector2.down, Mathf.Infinity, LayerMask.GetMask("Ground"));
                    if (rayInfo.transform == null)
                    {
                        rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                    }
                }
            }
        }
    }

    public void Attack()
    {
        StartCoroutine(AttackBridge());
    }

    private IEnumerator AttackBridge()
    {
        canMove = false;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9335f);
        AttackDamage();

        yield return new WaitForSeconds(2.75f);

        canMove = true;
    }

    private void AttackDamage()
    {
        Vector3 pos = new Vector3();

        if (spr.flipX == true)
        {
            pos = new Vector3(-2.75f, -2);
        }
        else if (spr.flipX == false)
        {
            pos = new Vector3(2.75f, -2);
        }

        RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position + pos, new Vector2(4.5f, 5), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));

        if (rayInfo.transform != null)
        {
            if (spr.flipX == true)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, -knockBack, 1);
            }
            else if (spr.flipX == false)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, knockBack, 1);
            }
        }
        speaker.PlaySound(attack, 0.75f, false);
    }
}
