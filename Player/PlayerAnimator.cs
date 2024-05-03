using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spr;
    private Animator anim;
    private PlayerController controller;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        controller = GetComponent<PlayerController>();
    }

    void Update()
    {
        // Flip sprite left and right, depending on which direction the player is moving
        if (Input.GetKeyDown(KeyCode.A) && transform.tag != "Dead")
        {
            spr.flipX = true;
        } else if (Input.GetKeyDown(KeyCode.D) && transform.tag != "Dead")
        {
            spr.flipX = false;
        }

        anim.SetFloat("xSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetBool("isGrounded", controller.isGrounded);
        anim.SetFloat("ySpeed", rb.velocity.y);

    }
}
