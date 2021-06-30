using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EagleComplex : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float nextWaypointDistance = 1f;
    public Transform enemyGX;
    Path path;
    int currentWaypoint = 0;
    Seeker seeker;
    private Animator anim;
    private float xPosOffset;
    private Vector3 initPos;
    private bool inInitPos;
    private bool reachedEndOfPath;
    private bool backPath;
    private Vector2 direction;
    private GridGraph grid;
    private Direction collisionHit;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        anim = enemyGX.GetComponent<Animator>();
        xPosOffset = enemyGX.localPosition.x;
        initPos = transform.position;
        inInitPos = true;
        backPath = false;
        direction = new Vector2(-1, 0);
        grid = (GridGraph)AstarPath.active.data.graphs[0];
        collisionHit = Direction.None;

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void FixedUpdate()
    {
        if (enemyGX.GetComponent<Enemy>().hit)
        {
            speed = 0;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<Seeker>().enabled = false;
            if (enemyGX.GetComponent<Enemy>().destroyed)
            {
                Destroy(gameObject);
            }
        }

        if (path == null)
        {
            if (collisionHit == Direction.Left)
            {
                direction = new Vector2(1, 0);
                collisionHit = Direction.None;
            }
            else if (collisionHit == Direction.Right)
            {
                direction = new Vector2(-1, 0);
                collisionHit = Direction.None;
            }

            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            if (currentWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
            }
            else
            {
                reachedEndOfPath = false;
            }

            if (!reachedEndOfPath)
            {
                direction = ((Vector2)path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
                transform.Translate(direction * speed * Time.deltaTime);

                float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }
            }

            if (reachedEndOfPath && backPath)
            {
                if(direction.y > 0 && transform.position.y < initPos.y || direction.y < 0 && transform.position.y > initPos.y)
                {
                    transform.Translate((new Vector2(transform.position.x,initPos.y) - new Vector2(transform.position.x, transform.position.y)).normalized * speed * Time.deltaTime);
                }
                else
                {
                    inInitPos = true;
                    backPath = false;
                    path = null;
                    direction = new Vector2(-1, 0);
                }
            }
        }


        if (direction.x > 0)
        {
            enemyGX.localScale = new Vector3(-1f, 1f, 1f);
            enemyGX.localPosition = new Vector3(-xPosOffset, enemyGX.localPosition.y, enemyGX.localPosition.z);
        }
        else if (direction.x < 0)
        {
            enemyGX.localScale = new Vector3(1f, 1f, 1f);
            enemyGX.localPosition = new Vector3(xPosOffset, enemyGX.localPosition.y, enemyGX.localPosition.z);
        }

    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            backPath = false;
            currentWaypoint = 0;
        }
    }

    void OnPathCompleteBack(Path p)
    {
        if (!p.error)
        {
            path = p;
            backPath = true;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            if (target.position.x < grid.center.x - 0.5f * grid.width || target.position.x > grid.center.x + 0.5f * grid.width)
            {
                if (!inInitPos)
                {
                    seeker.StartPath(new Vector2(transform.position.x, transform.position.y), new Vector2(initPos.x, initPos.y), OnPathCompleteBack);
                }

            }
            else
            {
                inInitPos = false;
                seeker.StartPath(new Vector2(transform.position.x, transform.position.y), target.position, OnPathComplete);
            }
        }

    }

    public void CollisionDetected(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && inInitPos)
        {
            if (direction.x < 0)
            {
                collisionHit = Direction.Left;
            }
            else if (direction.x > 0)
            {
                collisionHit = Direction.Right;
            }
        }
            
    }
}