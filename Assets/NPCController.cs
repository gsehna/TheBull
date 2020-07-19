using UnityEngine;
using Pathfinding;
using System.Collections;

public class NPCController : MonoBehaviour
{

    public enum NPCState {Wandering,Fleeing};

    public Animator animator;
    public Transform bull;
    public SpriteRenderer spriteRenderer;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;
    Seeker seeker;
    Rigidbody2D rb;
    public bool gettingPath = false;

    public NPCState state = NPCState.Wandering;

    [Header("Wandering")]
    public float wanderCDMin = 5;
    public float wanderCDMax = 10;
    public float wanderCDCurrent;
    public bool wanderCD = false;
    public float wanderSpeed = 2f;
    public float wanderDistanceMin = 5f;
    public float wanderDistanceMax = 20f;


    public float nextWaypointDistance = 3f;

    [Header("Fleeing")]
    public float fleeSpeed = 6f;
    public float fleeDistance = 20f;
    public float bullDistanceToFlee = 10;
    public bool startedFleeing = false;
    public bool shouldRun = false;



    private void Start()
    {
        bull = GameObject.Find("Bull").transform;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        WanderToRandom();
    }

    private void Update()
    {
        if (gettingPath)
        { return; }


        // Check distance to bull to see if it should be fleeing instead of wandering
        shouldRun = CheckForBullDistance();
        if(shouldRun)
        {
            state = NPCState.Fleeing;
        }
        else if (reachedEndofPath)
        {
            state = NPCState.Wandering;
            startedFleeing = false;
        }

        // Behaviour depending on state
        if(state == NPCState.Fleeing)
        {
            animator.SetBool("wandering",false);
            animator.SetBool("fleeing",true);

            if (!startedFleeing)
            {
                RunAway();
                startedFleeing = true;
                return;
            }

            if(path == null)
            { return; }

            if (currentWaypoint >= path.vectorPath.Count) // Reached the end of the current flee path, then check if needs to run again
            {
                reachedEndofPath = true;
                rb.velocity = Vector2.zero;
                if (shouldRun)
                {
                    RunAway();
                }
            }
            else
            {
                reachedEndofPath = false;
            }

        }
        else if (state == NPCState.Wandering && !wanderCD) // If wandering
        {
            animator.SetBool("wandering",true);
            animator.SetBool("fleeing",false);

            if (currentWaypoint >= path.vectorPath.Count) // Reached the end of the current wander path, then stop for some time and wander off again
            {
                reachedEndofPath = true;
                rb.velocity = Vector2.zero;

                wanderCDCurrent = Random.Range(wanderCDMin,wanderCDMax);
                wanderCD = true;

            }
            else
            {
                reachedEndofPath = false;
            }
        }
        else
        {
            
            animator.SetBool("wandering",false);
            animator.SetBool("fleeing",false);
        }
        
        // If state is wandering, there's some time the NPC will be standing in place before wandering off
        if(wanderCD && state == NPCState.Wandering)
        {
            if(wanderCDCurrent > 0)
            {
                wanderCDCurrent -= Time.deltaTime;
            }
            else
            {
                WanderToRandom();
                wanderCD = false;
            }
        }
    }


    private void FixedUpdate()
    {
        if(path == null || reachedEndofPath|| currentWaypoint > path.vectorPath.Count-1 || gettingPath)
        {
            return;
        }

        // Move to the current waypoint
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        float usedSpeed = state == NPCState.Wandering ? wanderSpeed : fleeSpeed;
        Vector2 force = direction * usedSpeed * Time.deltaTime;
        rb.AddForce(force);

        // Flip the spriter renderer depending on the direction it's going
        if (direction.x >= 0.01f) 
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x <= -0.01)
        {
            spriteRenderer.flipX = true;
        }

        // If reached current waypoint, move to the next one
        float distance = Vector2.Distance(rb.position,path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    bool CheckForBullDistance()
    {
        if (Vector2.Distance(rb.position,bull.position) <= bullDistanceToFlee)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void RunAway()
    {
        gettingPath = true;

        Vector2 fleeDirection = (rb.position - (Vector2)bull.position).normalized;
        Vector2 fleePosition =  fleeDirection*fleeDistance;
        seeker.StartPath(rb.position,fleePosition,OnPathComplete);
    }

    private void WanderToRandom()
    {
        gettingPath = true;

        Vector2 randomWander = new Vector2(Random.Range(-1,2)*Random.Range(wanderDistanceMin,wanderDistanceMax),Random.Range(-1,2)*Random.Range(wanderDistanceMin,wanderDistanceMax));
        Vector2 randomWanderPosition = rb.position + randomWander;
        seeker.StartPath(rb.position,randomWanderPosition,OnPathComplete);
        wanderCD = false;

    }


    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            gettingPath = false;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(transform.position, new Vector2(bullDistanceToFlee, bullDistanceToFlee));
    //}
}
