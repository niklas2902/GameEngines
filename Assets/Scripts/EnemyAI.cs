using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float nextWaypointDistance = 1.2f;
    public Transform enemyGX;
    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        anim = transform.GetChild(0).GetComponent<Animator>();

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        transform.Translate(force);

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if (force.x > 0)
        {
            enemyGX.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (force.x < 0)
        {
            enemyGX.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(new Vector2(transform.position.x, transform.position.y), target.position, OnPathComplete);
        }
        
    }

    public void Hit()
    {
        speed = 0;
        enemyGX.GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Seeker>().enabled = false;

        anim.SetBool("isDeath", true);
        Invoke(nameof(Kill), anim.GetCurrentAnimatorStateInfo(0).length);
    }

    void Kill()
    {
        Destroy(enemyGX.gameObject);
        Destroy(gameObject);
    }
}
