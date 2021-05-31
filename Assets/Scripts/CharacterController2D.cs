using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public Transform groundCheck;

    public float groundCheckRadius;

    public float jumpSpeed = 40;
    public float horizonalSpeed = 40;

    private Vector2 velocity;

    private bool wasGrounded = false;
    private bool isGrounded = false;

    public LayerMask groundMask;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = checkGrounded();

        rigidbody.velocity = velocity*Time.fixedDeltaTime;
    }

    bool checkGrounded()
    {
        return Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundMask).Length != 0;
    }

    public void Move(float horizontalMove, bool jump)
    {
        velocity = new Vector2(horizontalMove * horizonalSpeed, jump && !isGrounded ? jumpSpeed : 0);
        
    }
}
