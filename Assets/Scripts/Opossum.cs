using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossum : Enemy
{
    public float moveSpeed = 5f;
    private float direction = -1;

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (hit)
        {
            direction = 0;
            return;
        }

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
}
