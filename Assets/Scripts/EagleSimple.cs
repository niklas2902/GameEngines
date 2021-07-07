using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EagleSimple : Enemy
{
    private AIPath aiPath;

    void Start()
    {
        aiPath = gameObject.GetComponentInParent<AIPath>();
    }

    void FixedUpdate()
    {
        if (hit)
        {
            transform.parent.GetComponent<CircleCollider2D>().enabled = false;
            transform.parent.GetComponent<AIPath>().enabled = false;
            transform.parent.GetComponent<Seeker>().enabled = false;
            return;
        }

        Flip(aiPath.velocity);
    }
}
