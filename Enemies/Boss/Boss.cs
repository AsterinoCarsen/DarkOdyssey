using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    /*
     * 0 = standing still
     * 1 = chasing the player
     * 2 = teleporting
     * 3 = regenerating
     * 4 = attacking
     * 5 = dying
     * -1 = play next state
     */

    [Header("Double Slash Attack")]
    public float doubleSlashDMG;
    public Vector2 doubleSlashKnockback;

    [Header("Jump Slam Attack")]
    public float jumpSlamDMG;
    public Vector2 jumpSlamKnockback;

    [Header("Charge Attack")]
    public float chargeAttackDMG;
    public Vector2 chargeAttackKnockback;

    [Header("Heal")]
    public float healAmount;

    [Header("Movement")]
    public float moveSpeed;

    [Header("Audio")]
    public AudioClip basicAttack;
    public AudioClip jumpAttack;
    public AudioClip chargeAttack;
    public AudioClip regen;
    public AudioClip teleport;

    private int state = 0;
    private int attackState = 0;
    private Vector2 startPos;
    private bool canAttack = true;

    private Animator anim;
    private Transform player;
    private Enemy enemyScript;
    private Rigidbody2D rb;
    private Speaker speaker;
    private NewScene sceneScript;

    void Start()
    {
        sceneScript = GameObject.FindWithTag("Transition").GetComponent<NewScene>();
        anim = GetComponent<Animator>();
        enemyScript = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;
        speaker = GameObject.FindWithTag("Speaker").GetComponent<Speaker>();
        startPos = transform.position;
    }

    public void StartBoss()
    {
        InvokeRepeating("BossTimer", 0, 1);
    }

    void Update()
    {
        // Move to the player
        if (Vector2.Distance(transform.position, player.position) > 2 && state == 1)
        {
            if ((transform.position.x - player.position.x) > 0)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            } else if ((transform.position.x - player.position.x) < 0)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Vector2.Distance(transform.position, player.position) < 2)
        {
            Attack();
        }
    }

    private void BossTimer()
    {
        if (state == 0 || state == -1 || state == 1)
        {
            int rand = Random.Range(1, 60);

            if (rand == 1)
            {
                TryTeleport();
            }
            else if (rand == 2)
            {
                TryRegen();
            }
            else
            {
                state = 1;
            }
        }
    }

    private void TryTeleport()
    {
        if (state != 4 && state != 5 && state != 3)
        {
            anim.SetTrigger("Teleport");
        }
    }

    private void Teleport()
    {
        transform.position = startPos;
        speaker.PlaySound(teleport, 0.75f, false);
    }

    private void TryRegen()
    {
        if (state != 4 || state != 5)
        {
            StartCoroutine(Heal());
        }
    }

    private IEnumerator Heal()
    {
        state = 3;
        anim.SetTrigger("Heal");
        speaker.PlaySound(regen, 0.75f, false);

        float totalTime = anim.GetCurrentAnimatorClipInfo(0).Length;
        float elapsedTime = 0;

        while (elapsedTime < totalTime)
        {
            if (enemyScript.maxHealth < 1575)
            {
                enemyScript.maxHealth += healAmount / 542;
            }
            yield return new WaitForSeconds(0.01f);
            elapsedTime += Time.deltaTime;
        }
        state = -1;
    }

    private void Attack()
    {
        if (canAttack == true)
        {
            StartCoroutine(AttackBridge());
            StartCoroutine(AttackCoolDown());
        } else if (canAttack == false)
        {
            state = -1;
        }
    }

    private IEnumerator AttackBridge()
    {
        state = 4;
        attackState++;

        if (attackState == 1 || attackState == 2)
        {
            // Double Slash
            anim.SetTrigger("DoubleSlash");
            yield return new WaitForSeconds(0.1695f);
            DoubleSlashDamage();
            yield return new WaitForSeconds(0.5085f);
            DoubleSlashDamage();
        }
        else if (attackState == 3)
        {
            // Jump Slam
            anim.SetTrigger("JumpSlam");
            yield return new WaitForSeconds(0.084f);
            JumpSlamDamage();
            yield return new WaitForSeconds(0.4245f);
            JumpSlamDamage();
        }
        else if (attackState == 4)
        {
            // Charge
            anim.SetTrigger("Charge");
            yield return new WaitForSeconds(0.7909f);
            ChargeDamage();
            attackState = 0;
        }

        yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
        state = -1;
    }

    private void DoubleSlashDamage()
    {
        if ((transform.position.x - player.position.x) > 0)
        {
            RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position - new Vector3(2, 0), new Vector2(4, 0.5f), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
            if (rayInfo.transform != null)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(doubleSlashDMG / 2, new Vector2(-doubleSlashKnockback.x, doubleSlashKnockback.y), 0.25f);
            }
        }
        else if ((transform.position.x - player.position.x) < 0)
        {
            RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position + new Vector3(2, 0), new Vector2(4, 0.5f), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
            if (rayInfo.transform != null)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(doubleSlashDMG / 2, new Vector2(doubleSlashKnockback.x, doubleSlashKnockback.y), 0.25f);
            }
        }
        speaker.PlaySound(basicAttack, 0.75f, false);
    }

    private void JumpSlamDamage()
    {
        RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position, new Vector2(5, 0.5f), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
        if (rayInfo.transform != null)
        {
            if ((transform.position.x - player.position.x) > 0)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(jumpSlamDMG / 2, new Vector2(-jumpSlamKnockback.x, jumpSlamKnockback.y), 0.25f);
            } else if ((transform.position.x - player.position.x) < 0)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(jumpSlamDMG / 2, new Vector2(jumpSlamKnockback.x, jumpSlamKnockback.y), 0.25f);
            }
        }
        speaker.PlaySound(jumpAttack, 0.75f, false);
    }

    private void ChargeDamage()
    {
        if ((transform.position.x - player.position.x) > 0)
        {
            RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position - new Vector3(2, 0), new Vector2(4, 0.5f), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
            if (rayInfo.transform != null)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(chargeAttackDMG, new Vector2(-chargeAttackKnockback.x, chargeAttackKnockback.y), 0.25f);
            }
        }
        else if ((transform.position.x - player.position.x) < 0)
        {
            RaycastHit2D rayInfo = Physics2D.BoxCast(transform.position + new Vector3(2, 0), new Vector2(4, 0.5f), 0, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Player"));
            if (rayInfo.transform != null)
            {
                rayInfo.transform.GetComponent<PlayerController>().Damage(chargeAttackDMG, new Vector2(chargeAttackKnockback.x, chargeAttackKnockback.y), 0.25f);
            }
        }
        speaker.PlaySound(chargeAttack, 0.75f, false);
    }

    private IEnumerator AttackCoolDown()
    {
        canAttack = false;
        yield return new WaitForSeconds(2);
        canAttack = true;
    }

    public void Die()
    {
        sceneScript.ChangeSceneDelay(8, 3);
    }
}
