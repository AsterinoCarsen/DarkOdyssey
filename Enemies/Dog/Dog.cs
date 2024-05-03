using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public float alertDistance;
    public AudioClip attack;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator anim;
    private Speaker speaker;
    [HideInInspector]
    public bool canMove = true;

    public float moveSpeed;
    public float attackDamage;
    private float jumpSpeed = 12.5f;

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
                if ((player.position.x - transform.position.x) > 0.5f)
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
                        RaycastHit2D rayInfoDown = Physics2D.Raycast(transform.position + new Vector3(2.5f, 0), Vector2.down, Mathf.Infinity);
                        if (rayInfoDown.transform == null)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        } else if (rayInfoDown.transform != null)
                        {
                            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                        }

                        if (Vector2.Distance(transform.position, player.position) < 3.5f && Mathf.Abs((transform.position.y - player.position.y)) < 1)
                        {
                            Attack();
                        }
                    }
                }
                else if ((player.position.x - transform.position.x) < -0.5f)
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
                        RaycastHit2D rayInfoDown = Physics2D.Raycast(transform.position - new Vector3(2.5f, 0), Vector2.down, Mathf.Infinity);
                        if (rayInfoDown.transform == null)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                        }
                        else if (rayInfoDown.transform != null)
                        {
                            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                        }

                        if (Vector2.Distance(transform.position, player.position) < 3.5f && Mathf.Abs((transform.position.y - player.position.y)) < 1)
                        {
                            Attack();
                        }
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
        speaker.PlaySound(attack, 0.75f, false);

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.35f);
        RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position, new Vector2(5, 1), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));

        if (rayInfo.transform != null)
        {
            rayInfo.transform.GetComponent<PlayerController>().Damage(attackDamage, new Vector2(0, 20), 1);
        } else if (rayInfo.transform == null)
        {

        }

        rb.drag = 2.875f;

        yield return new WaitForSeconds(2);

        rb.drag = 0;

        canMove = true;
    }
}
