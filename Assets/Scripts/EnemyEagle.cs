using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().Sleep();

        //direction = 0;

        anim.SetBool("isDeath", true);
        Invoke(nameof(Kill), anim.GetCurrentAnimatorStateInfo(0).length);
    }

    void Kill()
    {
        Destroy(gameObject);
    }
}
