using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float alertDistance;

    [Header("Attack")]
    public float attackDamage;
    public float stunDuration;
    public Vector2 knockBack;

    [Header("Audio")]
    public AudioClip attack;

    [HideInInspector]
    public bool canMove;

    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Speaker speaker;
    private bool canAttack = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speaker = GameObject.FindWithTag("Speaker").GetComponent<Speaker>();

        canMove = true;
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
                    } else if (Vector2.Distance(player.position, transform.position) > 3 || canAttack == false)
                    {
                        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position + new Vector3(3, 0), Vector2.down, Mathf.Infinity);
                        if (rayInfo.transform != null)
                        {
                            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                        }
                    }

                } else if ((player.position.x - transform.position.x) < 0)
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
        canAttack = false;
        canMove = false;

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.6086f);
        AttackDamage();

        yield return new WaitForSeconds(3);

        canMove = true;
        canAttack = true;
    }

    private void AttackDamage()
    {
        RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position, new Vector2(12, 2), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
        if (rayInfo.transform != null)
        {
            if ((player.position.x - transform.position.x) > 0)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, knockBack, stunDuration);
            }
            else if ((player.position.x - transform.position.x) < 0)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, new Vector2(-knockBack.x, knockBack.y), stunDuration);
            }
        }
        speaker.PlaySound(attack, 0.75f, false);
    }
}
