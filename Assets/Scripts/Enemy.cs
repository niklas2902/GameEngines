using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public bool hit = false;
    public bool destroyed = false;
    private bool eagleComplex = false;
    private GameObject parent;

    private void Start()
    {
        parent = transform.parent.gameObject;
        if (transform.parent.GetComponent<EagleComplex>() != null)
        {
            eagleComplex = true;
        }
    }

    public void Hit()
    {
        GetComponent<AudioSource>().Play();
        GetComponent<BoxCollider2D>().enabled = false;

        if (GetComponent<Rigidbody2D>())
        {
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<Rigidbody2D>().Sleep();
        }

        hit = true;
        GetComponent<Animator>().SetBool("isDead", true);
        Invoke(nameof(Kill), GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

    void Kill()
    {
        Destroy(gameObject);
        if (parent)
        {
            Destroy(parent);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (eagleComplex)
        {
            transform.parent.GetComponent<EagleComplex>().CollisionDetected(collision);
        }
    }

    public void Flip(Vector2 movement)
    {
        if (movement.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (movement.x < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

}
