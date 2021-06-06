﻿using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public Transform groundCheck;

    public float groundCheckRadius;

    public float jumpSpeed = 40;
    public float horizonalSpeed = 40;
    public float movementSmoothing;

    private Vector2 velocity = Vector2.zero;

    private bool wasGrounded = false;
    private bool isGrounded = false;

    public LayerMask groundMask;

    public UnityEvent OnLandEvent = new UnityEvent();



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = checkGrounded();

        if(isGrounded && !wasGrounded && rigidbody.velocity.y <= 0)
        {
            OnLandEvent.Invoke();
        }
        wasGrounded = isGrounded;
    }

    bool checkGrounded()
    {
        return Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundMask).Length != 0;
    }

    public void Move(float horizontalMove, bool jump)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(horizontalMove * 10f, rigidbody.velocity.y);
        // And then smoothing it out and applying it to the character
        rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref velocity, movementSmoothing);

        if(jump && isGrounded)
        {
            rigidbody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }

        Flip(horizontalMove);   
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    void Flip(float horizontalMove)
    {
        if (horizontalMove < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (horizontalMove > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }
}