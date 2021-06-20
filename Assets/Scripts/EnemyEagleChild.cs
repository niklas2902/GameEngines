using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagleChild : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision Entered");
        transform.parent.GetComponent<EnemyAI>().CollisionDetected(collision);
    }
}
