using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Frog : Enemy
{
    private Animator anim;
    private Rigidbody2D rb;
    public float jumpSpeed = 35;
    public float waitTime = 5;
    public float radius = 10;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundMask;
    public LayerMask playerMask;

    private bool wasGrounded = false;
    private bool isGrounded = false;
    private bool jump = false;
    private float direction = 1;
    private bool timeToJump = false;
    private bool skipJump = false;

    public AudioClip jumpSound;
    private AudioSource audio;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        Invoke(nameof(CheckDirection), waitTime);
    }

    private void FixedUpdate()
    {
        isGrounded = checkGrounded();

        if (isGrounded && !wasGrounded)
        {
            anim.SetBool("isJumping", false);
            jump = false;
            anim.SetBool("jumpDown", false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        wasGrounded = isGrounded;

        if (timeToJump)
        {
            Jump();

            if (direction > 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else if (direction < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

        if (jump && rb.velocity.y < 0)
        {
            anim.SetBool("jumpDown", true);
        }

    }

    bool checkGrounded()
    {
        return Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundMask).Length != 0;
    }

    void Jump()
    {
        timeToJump = false;
        if (!skipJump)
        {
            audio.PlayOneShot(jumpSound);
            rb.AddForce(new Vector2(direction * jumpSpeed / 2, jumpSpeed), ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            jump = true;
        }
        skipJump = false;
        Invoke(nameof(CheckDirection), waitTime);
    }

    void CheckDirection()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radius, playerMask);
        if(playerCollider != null)
        {
            direction = transform.position.x < playerCollider.transform.position.x ? 1 : -1;
        } else
        {
            direction = direction == 1 ? -1 : 1;
            skipJump = true;
        }
        timeToJump = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
