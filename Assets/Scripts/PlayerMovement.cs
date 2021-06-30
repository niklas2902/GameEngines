using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMove = 0f;

    public float runSpeed = 40f;
    private CharacterController2D controller;

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
        if (Input.GetButtonDown("Jump") && !isDead)
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
        if (!isDead){
            controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        }
        jump = false;
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
        Invoke("ReloadLevel", reloadTime);
    }

    public void ReloadLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

public enum Direction
{
    Left,
    Right,
    None
}
