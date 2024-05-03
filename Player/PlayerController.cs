using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Speed at which player moves left and right on ground and in air.")]
    public float moveSpeed;
    [Tooltip("The Y velocity set when the player jumps.  Changing the gravity scale on Rigidbody2D, to something like 3 to 4, can help to make this feel more snappy.")]
    public float jumpSpeed;
    [Tooltip("Number of times the player can jump.")]
    public int jumps;
    [Range(0, 0.8f), Tooltip("How quickly the player slows down while not moving.  Works better if the player has a zero friction physics material.")]
    public float drag;
    [Range(0, 0.8f), Tooltip("How quickly the player speeds up to their desired velocity after pressing a key bind.  Works better if the player has a zero friction physics material.")]
    public float acceleration;

    [Header("Keybindings"), Space(10)]
    public KeyCode jumpKey;
    public KeyCode moveLeftKey;
    public KeyCode moveRightKey;

    [Header("Combat"), Space(10)]
    public float maxHealth;

    [Header("Misc"), Space(10)]
    public PhysicsMaterial2D antiSlide;
    public PhysicsMaterial2D playerSlide;

    [Header("Audio"), Space(10)]
    public AudioClip hurt;
    public AudioClip death;

    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public bool canMove = true;
    [HideInInspector]
    public bool canJump = true;
    int editorValueJumps;

    private Animator anim;
    private Rigidbody2D rb;
    private NewScene sceneChanger;
    private Collider2D playerCollider;
    private Speaker speaker;
    private bool dieOnce = false;
    private float defaultMoveSpeed;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sceneChanger = GameObject.FindWithTag("Transition").GetComponent<NewScene>();
        speaker = GameObject.FindWithTag("Speaker").GetComponent<Speaker>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        editorValueJumps = jumps;
        defaultMoveSpeed = moveSpeed;
        rb.angularDrag = 0;
    }

    void Update()
    {
        if (isGrounded == true)
        {
            moveSpeed = defaultMoveSpeed;
        } else if (isGrounded == false)
        {
            moveSpeed = defaultMoveSpeed * 1.05f;
        }

        if (transform.position.y < -30f)
        {
            Die(true);
        }

        // Moving
        if (canMove == true)
        {
            if (Input.GetKey(moveLeftKey))
            {
                Vector2 vel = rb.velocity;
                vel.x = Mathf.Lerp(vel.x, -moveSpeed, acceleration);
                rb.velocity = vel;
            }
            else if (Input.GetKey(moveRightKey))
            {
                Vector2 vel = rb.velocity;
                vel.x = Mathf.Lerp(vel.x, moveSpeed, acceleration);
                rb.velocity = vel;
            }
            else if (!Input.GetKey(moveLeftKey) && !Input.GetKey(moveRightKey))
            {
                Vector2 vel = rb.velocity;
                vel.x = Mathf.Lerp(vel.x, 0, drag);
                rb.velocity = vel;
            }

            if (Input.GetKeyDown(jumpKey) && jumps > 0 && canJump == true)
            {
                Vector2 vel = rb.velocity;
                vel.y = jumpSpeed;
                rb.velocity = vel;

                anim.SetTrigger("Jump");

                jumps -= 1;
            }

            if (rb.IsTouchingLayers(LayerMask.GetMask("Ground")) && !Input.GetKey(moveLeftKey) && !Input.GetKey(moveRightKey))
            {
                playerCollider.sharedMaterial = antiSlide;
            }
            else
            {
                playerCollider.sharedMaterial = playerSlide;
            }
        }
    }

    public void SetGround(bool groundBool)
    {
        isGrounded = groundBool;
        if (groundBool == true)
        {
            jumps = editorValueJumps;
        }
    }

    public void Damage(float damage, Vector2 force, float duration)
    {
        StartCoroutine(DamageBridge(damage, force, duration));
    }

    private IEnumerator DamageBridge(float damage, Vector2 force, float duration)
    {
        canMove = false;
        rb.velocity = force;
        speaker.PlaySound(hurt, 0.75f, false);

        maxHealth -= damage;
        anim.SetTrigger("Hit");
        if (maxHealth <= 0)
        {
            Die(false);
        }

        // duration temporarily disabled to fixed 0.25s, for less stun duration, more interaction
        yield return new WaitForSeconds(0.25f);

        if (maxHealth > 0)
        {
            canMove = true;
        }
    }

    public void Die(bool isInstant)
    {
        if (isInstant == true)
        {
            sceneChanger.ReloadCurrentScene();
            canMove = false;
        } else if (isInstant == false)
        {
            StartCoroutine(DieBridge());
        }
    }

    private IEnumerator DieBridge()
    {
        if (dieOnce == false)
        {
            dieOnce = true;
            speaker.PlaySound(death, 0.75f, false);

            canMove = false;
            anim.SetTrigger("Die");
            Destroy(GetComponent<PlayerAttack>());
            gameObject.layer = 2;
            transform.tag = "Dead";
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
            rb.velocity = new Vector2(0, rb.velocity.y);

            yield return new WaitForSeconds(2.5f);
            sceneChanger.ReloadCurrentScene();
        }
    }
}
