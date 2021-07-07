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

    private bool playerJumped = false;
    private bool isGrounded = false;

    public LayerMask groundMask;

    public UnityEvent OnLandEvent = new UnityEvent();

    private float startGravityScale; // remove gravity, when player climbs on ladder
    public float onGroundThreshold = 0.01f;

    private bool wasGrounded;
    public bool Grounded { get => isGrounded; }



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

        if(isGrounded && playerJumped && rigidbody.velocity.y <= onGroundThreshold && !wasGrounded)
        {
            playerJumped = false;
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
        if (isClimbing) {
            rigidbody.gravityScale = 0;
        }
        else
        {
            rigidbody.gravityScale = startGravityScale;
        }

        //From:https://github.com/Brackeys/2D-Character-Controller/blob/master/CharacterController2D.cs
        // Move the character by finding the target velocity
        Vector3 targetVelocity = new Vector2(horizontalMove * 10f, !isClimbing ? rigidbody.velocity.y : verticalMove * 10f);
        // And then smoothing it out and applying it to the character
        rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref velocity, movementSmoothing);



        if(jump && isGrounded)
        {
            playerJumped = true;
            rigidbody.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
            GetComponent<AudioSource>().Play();
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
