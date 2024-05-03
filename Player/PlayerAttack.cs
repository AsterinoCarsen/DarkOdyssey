using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack1"), Space(10)]
    public float attack1CoolDown;
    public float attack1Damage;
    public float attack1StunDuration;
    public Vector2 attack1Knockback;

    [Header("Attack2"), Space(10)]
    public float attack2CoolDown;
    public float attack2Damage;
    public float attack2StunDuration;
    public Vector2 attack2Knockback;

    [Header("Attack3"), Space(10)]
    public float attack3CoolDown;
    public float attack3Damage;
    public float attack3StunDuration;
    public Vector2 attack3Knockback;

    [Header("Misc"), Space(10)]
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private PlayerController controller;
    private PlayerIndicators indicator;
    private Speaker speaker;

    private bool canAttack1 = true;
    private bool canAttack2 = true;
    private bool canAttackDodge = true;
    [HideInInspector]
    public bool isSlicing = false;

    void Start()
    {
        indicator = GameObject.FindWithTag("Indicator").GetComponent<PlayerIndicators>();
        controller = GetComponent<PlayerController>();
        speaker = GameObject.FindWithTag("Speaker").GetComponent<Speaker>();
        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Initiate the attacks
        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack1 == true)
        {
            anim.SetTrigger("Attack1");
            indicator.StartCD(1, attack1CoolDown);
            Attack1();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && canAttack2 == true)
        {
            anim.SetTrigger("Attack2");
            indicator.StartCD(2, attack2CoolDown);
            Attack2();
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift) && canAttackDodge == true)
        {
            anim.SetTrigger("AttackDodge");
            indicator.StartCD(3, attack3CoolDown);
            AttackDodge();
        }
    }

    public void Attack1Damage()
    {
        speaker.PlaySound(attack1, 0.75f, false);
        if (spr.flipX == true)
        {
            RaycastHit2D[] rayInfo = Physics2D.BoxCastAll(transform.position + new Vector3(-1, 0), new Vector2(2, 2), 0, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D i in rayInfo)
            {
                i.transform.GetComponent<Enemy>().Damage(attack1Damage, attack1StunDuration, new Vector2(-attack1Knockback.x, attack1Knockback.y));
            }
        } else if (spr.flipX == false)
        {
            RaycastHit2D[] rayInfo = Physics2D.BoxCastAll(transform.position + new Vector3(1, 0), new Vector2(2, 2), 0, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D i in rayInfo)
            {
                i.transform.GetComponent<Enemy>().Damage(attack1Damage, attack1StunDuration, new Vector2(attack1Knockback.x, attack1Knockback.y));
            }
        }
    }

    public void Attack2Damage()
    {
        speaker.PlaySound(attack2, 0.75f, false);
        if (spr.flipX == true)
        {
            RaycastHit2D[] rayInfo = Physics2D.BoxCastAll(transform.position + new Vector3(-1, 0), new Vector2(2, 2), 0, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D i in rayInfo)
            {
                i.transform.GetComponent<Enemy>().Damage(attack2Damage, attack2StunDuration, new Vector2(-attack2Knockback.x, attack2Knockback.y));
            }
        }
        else if (spr.flipX == false)
        {
            RaycastHit2D[] rayInfo = Physics2D.BoxCastAll(transform.position + new Vector3(1, 0), new Vector2(2, 2), 0, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D i in rayInfo)
            {
                i.transform.GetComponent<Enemy>().Damage(attack2Damage, attack2StunDuration, new Vector2(attack2Knockback.x, attack2Knockback.y));
            }
        }
    }

    public void Attack3Damage()
    {
        speaker.PlaySound(attack3, 0.75f, false);
        if (spr.flipX == true)
        {
            RaycastHit2D[] rayInfo = Physics2D.BoxCastAll(transform.position + new Vector3(0, 0), new Vector2(4, 2), 0, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D i in rayInfo)
            {
                i.transform.GetComponent<Enemy>().Damage(attack3Damage, attack3StunDuration, new Vector2(-attack3Knockback.x, attack3Knockback.y));
            }
        }
        else if (spr.flipX == false)
        {
            RaycastHit2D[] rayInfo = Physics2D.BoxCastAll(transform.position + new Vector3(0, 0), new Vector2(4, 2), 0, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Enemy"));
            foreach (RaycastHit2D i in rayInfo)
            {
                i.transform.GetComponent<Enemy>().Damage(attack3Damage, attack3StunDuration, new Vector2(attack3Knockback.x, attack3Knockback.y));
            }
        }
    }

    public IEnumerator StartAttack3()
    {
        isSlicing = true;
        controller.canJump = false;
        gameObject.layer = 0;
        Physics2D.IgnoreLayerCollision(0, 8);

        yield return new WaitForSeconds(1.5f);
        EndAttack3();
    }

    public void EndAttack3()
    {
        isSlicing = false;
        controller.canJump = true;
        gameObject.layer = 9;
    }


    // Individual attacks have seperate cooldowns
    private void Attack1()
    {
        StartCoroutine(CoolDown(attack1CoolDown, 1));
    }

    private void Attack2()
    {
        StartCoroutine(CoolDown(attack2CoolDown, 2));
    }

    private void AttackDodge()
    {
        StartCoroutine(CoolDown(attack3CoolDown, 3));
    }

    private IEnumerator CoolDown(float duration, int type)
    {
        if (type == 1)
        {
            canAttack1 = false;
            yield return new WaitForSeconds(duration);
            canAttack1 = true;
        } else if (type == 2)
        {
            canAttack2 = false;
            yield return new WaitForSeconds(duration);
            canAttack2 = true;
        } else if (type == 3)
        {
            canAttackDodge = false;
            yield return new WaitForSeconds(duration);
            canAttackDodge = true;
        }
    }
}
