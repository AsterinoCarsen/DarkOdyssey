using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
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

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spr;
    private Speaker speaker;

    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool canAttack = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        speaker = GameObject.FindWithTag("Speaker").GetComponent<Speaker>();
    }

    void Update()
    {
        if (canMove == true)
        {
            if (Vector2.Distance(player.position, transform.position) < alertDistance)
            {
                if ((player.position.x - transform.position.x) > 0)
                {
                    if (Vector2.Distance(player.position, transform.position) < 3 && canAttack == true)
                    {
                        StartCoroutine(Attack());
                    }
                    else if (Vector2.Distance(player.position, transform.position) > 3 || canAttack == false)
                    {
                        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position + new Vector3(3, 0), Vector2.down, Mathf.Infinity);
                        if (rayInfo.transform != null)
                        {
                            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                        }
                    }

                }
                else if ((player.position.x - transform.position.x) < 0)
                {
                    if (Vector2.Distance(player.position, transform.position) < 3 && canAttack == true)
                    {
                        StartCoroutine(Attack());
                    }
                    else if (Vector2.Distance(player.position, transform.position) > 3 || canAttack == false)
                    {
                        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position - new Vector3(3, 0), Vector2.down, Mathf.Infinity);
                        if (rayInfo.transform != null)
                        {
                            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                        }
                    }
                }
            }
        }
    }

    private IEnumerator Attack()
    {
        if (canAttack == true)
        {
            canAttack = false;
            canMove = false;

            anim.SetTrigger("Attack");

            yield return new WaitForSeconds(0.4501f);
            AttackDamage();

            yield return new WaitForSeconds(2);

            canMove = true;
            canAttack = true;
        }
    }

    private void AttackDamage()
    {
        if (spr.flipX == true)
        {
            RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position - new Vector3(3f, 0), new Vector2(4, 2), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
            if (rayInfo.transform != null)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, new Vector2(-knockBack.x, knockBack.y), stunDuration);
            }
        } else if (spr.flipX == false)
        {
            RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position + new Vector3(3f, 0), new Vector2(4, 2), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
            if (rayInfo.transform != null)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, new Vector2(knockBack.x, knockBack.y), stunDuration);
            }
        }
        speaker.PlaySound(attack, 0.75f, false);
    }
}
