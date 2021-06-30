using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EagleSimple : Enemy
{
    public AIPath aiPath;

    void FixedUpdate()
    {
        if (hit)
        {
            transform.parent.GetComponent<CircleCollider2D>().enabled = false;
            transform.parent.GetComponent<AIPath>().enabled = false;
            transform.parent.GetComponent<Seeker>().enabled = false;
            return;
        }

        if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        } 
        else if (aiPath.desiredVelocity.x <= 0.01f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
