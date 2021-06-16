using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();   
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger("Activate");
    }

    private void Remove()
    {
        Destroy(gameObject);
    }

}
