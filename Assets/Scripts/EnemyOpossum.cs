using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOpossum : MonoBehaviour
{
    public float moveSpeed = 5f;
    private float direction = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(direction, 0f, 0f);
        if(movement.x > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        } else
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        transform.position += movement * Time.deltaTime * moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Obstacles")
        {
            direction = direction == -1 ? 1 : -1;
        }
    }
}
