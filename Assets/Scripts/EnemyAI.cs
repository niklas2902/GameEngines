using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
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
 

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        anim = enemyGX.GetComponent<Animator>();
        xPosOffset = enemyGX.localPosition.x;
        initPos = transform.position;
        inInitPos = true;
        backPath = false;
        direction = new Vector2(-1, 0);

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if(path == null)
        {
            if(transform.position.x < -7)
            {
                direction = (new Vector2(7, initPos.y) - new Vector2(0, transform.position.y)).normalized;
            }
            else if (transform.position.x > 7)
            {
                direction = (new Vector2(-7, initPos.y) - new Vector2(0, transform.position.y)).normalized;
            }

            transform.Translate(direction * speed * Time.deltaTime);
        } else
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
                Vector2 movement = direction * speed * Time.deltaTime;

                transform.Translate(movement);

                float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }



            }

            if (reachedEndOfPath && backPath)
            {
                inInitPos = true;
                backPath = false;
                path = null;
                direction = (new Vector2(-7, initPos.y) - new Vector2(0, transform.position.y)).normalized;


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
            if (target.position.x < -12 || target.position.x > 11)
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
