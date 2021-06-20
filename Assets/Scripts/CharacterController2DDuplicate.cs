using UnityEngine;
using UnityEngine.Events;

public class CharacterController2DDuplicate : MonoBehaviour
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

    private float startGravityScale;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        startGravityScale = rigidbody.gravityScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isGrounded = checkGrounded();

        if(isGrounded && !wasGrounded)
        {
            OnLandEvent.Invoke();
        }
        wasGrounded = isGrounded;
    }

    bool checkGrounded()
    {
        return Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundMask).Length != 0;
    }

    public void Move(float horizontalMove, float verticalMove, bool jump, bool isClimbing)
    {
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(horizontalMove * 10f, !isClimbing?rigidbody.velocity.y:verticalMove * 10f);
        if (isClimbing) {
            rigidbody.gravityScale = 0;
        }
        else
        {
            rigidbody.gravityScale = startGravityScale;
        }
        // And then smoothing it out and applying it to the character
        rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref velocity, movementSmoothing);

        if(jump && isGrounded)
        {
            rigidbody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        }

        if (!isClimbing)
        {
            Flip(horizontalMove);
        }
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
