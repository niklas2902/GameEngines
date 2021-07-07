using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EagleComplex : MonoBehaviour
{
    public Transform target;
    public Transform enemySprite;
    public float speed = 2.5f;
    public float waypointDistance = 1f;
    public float repathRate = 0.5f;

    private Seeker seeker;
    private Animator anim;
    private GridGraph grid;

    private Path path;
    private int currentWaypoint = 0;
    private bool endOfPath;
    
    private Vector3 initPos;
    private bool inInitMovement = true;
    private bool backPath = false;

    private Vector2 direction;
    private Direction collisionHit = Direction.None;
    
    void Start()
    {
        seeker = GetComponent<Seeker>();
        anim = enemySprite.GetComponent<Animator>();
        grid = (GridGraph)AstarPath.active.data.graphs[0];

        initPos = transform.position;
        direction = new Vector2(-1, 0);

        InvokeRepeating(nameof(UpdatePath), 0f, repathRate);
    }

    void FixedUpdate()
    {
        if (enemySprite.GetComponent<Enemy>().hit)
        {
            speed = 0;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<Seeker>().enabled = false;
            if (enemySprite.GetComponent<Enemy>().destroyed)
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
            endOfPath = currentWaypoint >= path.vectorPath.Count ? true : false;

            if (!endOfPath)
            {
                direction = ((Vector2) path.vectorPath[currentWaypoint] - new Vector2(transform.position.x, transform.position.y)).normalized;
                transform.Translate(direction * speed * Time.deltaTime);

                float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), path.vectorPath[currentWaypoint]);

                if (distance < waypointDistance)
                {
                    currentWaypoint++;
                }
            }

            if (endOfPath && backPath)
            {
                if(direction.y > 0 && transform.position.y < initPos.y || direction.y < 0 && transform.position.y > initPos.y)
                {
                    transform.Translate((new Vector2(transform.position.x,initPos.y) - new Vector2(transform.position.x, transform.position.y)).normalized * speed * Time.deltaTime);
                }
                else
                {
                    inInitMovement = true;
                    backPath = false;
                    path = null;
                    direction = new Vector2(-1, 0);
                }
            }
        }

        enemySprite.GetComponent<Enemy>().Flip(direction);

    }

    void UpdatePath()
    {
        bool playerIsDead = target.GetComponent<PlayerMovementComplex>() ? target.GetComponent<PlayerMovementComplex>().playerIsDead() : target.GetComponent<PlayerMovement>().playerIsDead();

        if (seeker.IsDone() && !playerIsDead)
        {
            if (target.position.x < grid.center.x - 0.5f * grid.width || target.position.x > grid.center.x + 0.5f * grid.width) 
            {
                if (!inInitMovement)
                {
                    seeker.StartPath(new Vector2(transform.position.x, transform.position.y), new Vector2(initPos.x, initPos.y), OnBackPathComplete);
                }
            }
            else
            {
                inInitMovement = false;
                seeker.StartPath(new Vector2(transform.position.x, transform.position.y), target.position, OnPathComplete);
            }
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

    void OnBackPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            backPath = true;
            currentWaypoint = 0;
        }
    }

    public void CollisionDetected(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" && inInitMovement)
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