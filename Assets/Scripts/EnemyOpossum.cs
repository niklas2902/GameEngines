using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOpossum : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float direction = -1;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(direction, 0f, 0f);
        if(movement.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        } else if(movement.x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position += movement * Time.deltaTime * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacles")
        {
            direction = direction == -1 ? 1 : -1;
        }
    }

    public void Hit()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().Sleep();

        direction = 0;

        anim.SetBool("isDeath", true);
        Invoke(nameof(Kill), anim.GetCurrentAnimatorStateInfo(0).length);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
