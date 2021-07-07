using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementDuplicate : MonoBehaviour
{
    private float horizontalMove = 0f;
    private float verticalMove;

    public float runSpeed = 40f;
    public float climbSpeed = 10f;

    private CharacterController2DDuplicate controller;

    public Transform ladderCollider;

    public LayerMask ladderMask;

    public Transform hitCheck;

    public float hitCheckRadius;

    public float hitForce = 10;

    public float upwardsHitForce = 10;
    public float sidewardsHitForce = 10;

    public readonly float reloadTime = 1;

    private bool isDead = false;

    private Animator animator;

    bool jump;

    public LayerMask enemyLayer;

    public Collider2D colliderNomal;
    public Collider2D colliderCrouch;

    public bool isCrouching;

    private bool isClimbing = false;

    private bool inLadderZone;
    public bool InLadderZone { get {
            return inLadderZone;
        } set { inLadderZone = value;
            isClimbing = false;
        } }

    public bool Won { get; set; } = false;

    public AudioClip jumpSound;
    public AudioClip deathSound;
    private AudioSource audio;

    private void Start()
    {
        controller = GetComponent<CharacterController2DDuplicate>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
        if (verticalMove < 0 && !isDead)
        {
            if (!isClimbing) // As we are using the same keys for crouching and climbing down, we have to distinguish the current situation
            {
                animator.SetBool("IsCrouching", true);
                animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
                colliderCrouch.enabled = true;
                colliderNomal.enabled = false;
                isCrouching = true;
                horizontalMove = 0;
            }
            else
            {
                verticalMove = -climbSpeed;
            }
        }

        else if (!isDead && !Won) // Resetting the crouching
        {
            animator.SetBool("IsCrouching", false);
            colliderCrouch.enabled = false;
            colliderNomal.enabled = true;
            isCrouching = false;
        }

        if (inLadderZone)
        {
            animator.SetBool("isClimbing", true);
            if(verticalMove != 0)
            {
                animator.SetBool("IsJumping", false);
                isClimbing = true;
            }
        }
        else {
            isClimbing = false;
            animator.SetBool("isClimbing", false); 
        }

        if (!isCrouching)
        { // Normal move to on the horizontal and handling jumping
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
            if (Input.GetButtonDown("Jump") && !isDead && controller.Grounded && !isClimbing) // player shouldn't be able to jump, when on ladder
            {
                audio.PlayOneShot(jumpSound);
                animator.SetBool("IsJumping", true);
                jump = true;
            }
        }
        if (Won)
        {
            animator.SetFloat("Speed", 0);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(hitCheck.position, hitCheckRadius, enemyLayer);

            // Checking whether player hit at bottom
            if (colliders.Length > 0) {
                colliders[0].gameObject.GetComponent<Enemy>().Hit();
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 0);
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, hitForce), ForceMode2D.Impulse);
            }
            else
            {
                Hit();
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
        if (!isDead && !Won){
            if (isClimbing && verticalMove == 0 && horizontalMove == 0) {
                animator.speed = 0;
            }
            else
            {
                animator.speed = 1;
            }

            if (!CheckLadder() && verticalMove > 0) {
                verticalMove = 0;
            }
            controller.Move(horizontalMove * Time.fixedDeltaTime,verticalMove * Time.fixedDeltaTime, jump, isClimbing);
        }
        jump = false;
    }

    bool CheckLadder()
    {
        //Function for player not to move above ladder
        return Physics2D.OverlapCircleAll(ladderCollider.position, 0.5f, ladderMask).Length != 0;
    }

    public void StopJump()
    {
        animator.SetBool("IsJumping", false);
    }

    public void Hit()
    {
        isDead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        animator.SetBool("Hit", true);
        audio.PlayOneShot(deathSound);
        Invoke("ReloadLevel", deathSound.length);
    }

    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlaySound(AudioClip clip)
    {
        audio.PlayOneShot(clip);
    }
}
