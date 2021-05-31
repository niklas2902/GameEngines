using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMove = 0f;

    public float runSpeed = 40f;

    public CharacterController2D controller;

    public Transform hitCheck;

    public float hitCheckRadius;

    public float hitForce = 10;

    public float upwardsHitForce = 10;
    public float sidewardsHitForce = 10;

    private Animator animator;

    bool jump;

    public LayerMask enemyLayer;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("IsJumping", true);
            jump = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(hitCheck.position, hitCheckRadius, enemyLayer);

            // Checking whether player hit at bottom
            if (colliders.Length > 0) {
                colliders[0].gameObject.GetComponent<EnemyOpossum>().Hit();
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, hitForce), ForceMode2D.Impulse);
            }
            else
            {
                Debug.Log("Hit by Enemy");
            }

        }
    }

    private void OnDrawGizmos()
    {
        if (hitCheck != null)
        {
            Gizmos.DrawWireSphere(hitCheck.position, hitCheckRadius);
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    public void StopJump()
    {
        animator.SetBool("IsJumping", false);
    }

    public void Hit(Direction direction)
    {
        
         GetComponent<Rigidbody2D>().AddForce(new Vector2(sidewardsHitForce*
             (direction == Direction.Left?1:-1),upwardsHitForce ));
        Debug.Log("Hit");
    }
}

public enum Direction
{
    Left,
    Right
}
