using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electric : MonoBehaviour
{
    [Header("Movement")]
    public float alertDistance;
    public float moveSpeed = 2;

    [Header("Attack")]
    public float attackDamage;
    public float stunDuration;
    [Range(0.75f, 5f)]
    public float attackSpeed;
    public Vector2 knockBack;

    [Header("Audio")]
    public AudioClip attack;

    private float jumpSpeed = 10;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator anim;
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
            // Inside alert distance
            if (Vector2.Distance(transform.position, player.position) < alertDistance)
            {
                // Which direction
                if ((player.position.x - transform.position.x) > 0)
                {
                    // Move right
                    RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Vector2.right, 1, LayerMask.GetMask("Ground"));
                    RaycastHit2D rayInfoUp = Physics2D.Raycast(transform.position, Vector2.up, 3, LayerMask.GetMask("Ground"));
                    if (rayInfo.transform != null && rayInfoUp.transform == null)
                    {
                        rb.velocity = new Vector2(moveSpeed, jumpSpeed);
                    }
                    else
                    {
                        RaycastHit2D rayInfoFall = Physics2D.Raycast(transform.position + new Vector3(2.5f, 0), Vector2.down, Mathf.Infinity);
                        if (rayInfoFall.transform == null)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        } else if (rayInfoFall.transform != null)
                        {
                            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                        }

                        if (Vector2.Distance(transform.position, player.position) < 3.5f)
                        {
                            Attack();
                        }
                    }
                }
                else if ((player.position.x - transform.position.x) < 0)
                {
                    // Move left
                    RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Vector2.left, 1, LayerMask.GetMask("Ground"));
                    RaycastHit2D rayInfoUp = Physics2D.Raycast(transform.position, Vector2.up, 3, LayerMask.GetMask("Ground"));
                    if (rayInfo.transform != null && rayInfoUp.transform == null)
                    {
                        rb.velocity = new Vector2(-moveSpeed, jumpSpeed);
                    }
                    else
                    {
                        RaycastHit2D rayInfoFall = Physics2D.Raycast(transform.position - new Vector3(2.5f, 0), Vector2.down, Mathf.Infinity);
                        if (rayInfoFall.transform == null)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                        else if (rayInfoFall.transform != null)
                        {
                            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                        }

                        if (Vector2.Distance(transform.position, player.position) < 3.5f)
                        {
                            Attack();
                        }
                    }
                }
            }
        } else if (canMove == false)
        {
            rb.velocity = new Vector2(0, 0);
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
        yield return new WaitForSeconds(0.533f);

        AttackDamage();

        yield return new WaitForSeconds(attackSpeed);

        canAttack = true;
        canMove = true;
    }

    private void AttackDamage()
    {
        if (canAttack == true)
        {
            speaker.PlaySound(attack, 0.75f, false);
            canAttack = false;
            if (spr.flipX == true)
            {
                // Left
                RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position - new Vector3(2.5f, 0), new Vector2(5, 2), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
                if (rayInfo.transform != null)
                {
                    rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, new Vector2(-knockBack.x, knockBack.y), stunDuration);
                }
            }
            else if (spr.flipX == false)
            {
                // Right
                RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position + new Vector3(2.5f, 0), new Vector2(5, 2), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
                if (rayInfo.transform != null)
                {
                    rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, new Vector2(knockBack.x, knockBack.y), stunDuration);
                }
            }
        }
    }
}
