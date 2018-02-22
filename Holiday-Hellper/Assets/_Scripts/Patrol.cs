using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Patrol - Walks between set points
//Pursuing - Chases player
//Similar to Patrol, but now radius to notice player is much larger 
public enum PatrolState { PATROLLING, PURSUING, VIGILANT, WANDER, SEARCH, STUNNED };

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]

public class Patrol : MonoBehaviour
{
    [SpaceAttribute]

    public float fovAngle;
    public GameObject target;

    [SpaceAttribute]

    public Transform[] patrolPoints;

    private int wanderIndex;
    public NavMeshAgent agent;
    private Vector3 directionTotarget;
    public SphereCollider hearingRadius;
    public float distance;

    [SpaceAttribute]
    public float walkSpeed;
    public float runSpeed;

    public Animator anim;
    [SpaceAttribute]
    public float normalRad;
    public float vigilantRad;

    public float viewDistance;

    public float normalFov;
    public float vigilantFov;

    public float vigilantTime = 5f;
    public float wanderDistance;
    [SpaceAttribute]
    public bool alerted; //Used to swap us between patrol and vigilant 
                         //Alerted triggered after pursuing state set 
                         //Uses coroutine to turn alerted off

    public bool search; //similar to alerted 
    public bool stunned;
    public bool canHear;
    public bool canSee;

    public bool wander;

    public LayerMask viewMask;

    private bool playerHidden;

    private PlayerState playersState;

    public PatrolState _patrolState;

    private GameState gameState = GameState.PLAYING;
    [SpaceAttribute]

    public Vector3 offsetVector;


    // Use this for initialization
    private void Start()
    {
        _patrolState = PatrolState.PATROLLING;
        wanderIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = Random.Range(1, 100);
        hearingRadius = GetComponent<SphereCollider>();
        alerted = false;

    }

    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        PlayerController.CurrentState += playerState;
        Stun.stun += stunPatrol;
    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        PlayerController.CurrentState -= playerState;
        Stun.stun -= stunPatrol;
    }

    void Update()
    {

        //Stop the patrol 
        if (gameState == GameState.PAUSED || gameState == GameState.WIN)
        {
            agent.isStopped = true;
            return;
        }
        else
        {
            agent.isStopped = false;
        }

        //Calculate the distance between the patrol and the player 
        distance = Vector3.Distance(transform.position, target.transform.position);

        //If the player is hiding then patrol can't see or hear them 
        if (playersState == PlayerState.HIDE)
        {
            canHear = false;
            canSee = false;
        }

        //the local scale of the z will mess with the hearingRadius' radius, so multiple by z
        viewDistance = hearingRadius.radius * transform.localScale.z;

        //Swap between states
        changeState();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            //WITHOUT THAT STUPID OFFSET VECTOR PATROL WILL AIM AT PLAYERS FEET AND RUIN EVERYTHING
            directionTotarget = ((target.transform.position + offsetVector) - transform.position);
            //if the player enters our hearing field we can only hear them if they aren't sneaking
            if (playersState == PlayerState.SNEAK)
            {
                canSee = CanSeePlayer(directionTotarget);
                return;
            }

            canSee = CanSeePlayer(directionTotarget); //checks to see if player is in field of view
            canHear = CanHearPlayer(directionTotarget); //checks to see if anything is obstructing hearing radius, patrol can't hear through walls with this
        }
    }

    // if the target gets out of range while chasing them
    // go back to wandering/patrolling
    private void OnTriggerExit(Collider other)
    {
        canHear = false;
        if (_patrolState == PatrolState.PURSUING)
        {

            if (other.gameObject == target)
            {
                alerted = true;
                _patrolState = PatrolState.PATROLLING;

            }
        }
    }



    // chases after the target
    private void Pursue()
    {

        if (_patrolState == PatrolState.PURSUING)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    //Recieve notification from GameController
    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    //Set the next destination point 
    void getNewDestination()
    {
        wanderIndex++;

        if (wanderIndex >= patrolPoints.Length)
        {
            wanderIndex = 0;
        }
        agent.SetDestination(patrolPoints[wanderIndex].transform.position);
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;
        randomDirection = new Vector3(randomDirection.x, origin.y, randomDirection.z);
        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    //Change states and control what is done in those states
    void changeState()
    {
        switch (_patrolState)
        {
            case PatrolState.PATROLLING:
                if (wander) { _patrolState = PatrolState.WANDER; }
                //Set the fov
                fovAngle = normalFov;
                //Change the speed
                agent.speed = walkSpeed;
                //Changes the animation depending on the speed the agent is moving
                anim.SetFloat("BlendX", agent.velocity.x);
                anim.SetFloat("BlendY", agent.velocity.z);
                //set sphere collider radius back to normal
                hearingRadius.radius = normalRad;
                //move the patrol 
                if (agent.remainingDistance <= 1 || agent.destination == null)
                { getNewDestination(); }
                //if patrol can see or hear player then pursue them
                if (canSee || canHear)
                {
                    _patrolState = PatrolState.PURSUING;
                }
                //swap to vigilant
                if (alerted)
                {
                    //change state to vigilant then start coroutine
                    _patrolState = PatrolState.VIGILANT;
                    StartCoroutine(CountDown(vigilantTime, alerted));
                }
                //swap to search
                if (search)
                {
                    //change state to search then start coroutine
                    _patrolState = PatrolState.SEARCH;
                    StartCoroutine(CountDown(vigilantTime, search));
                }

                break;

            case PatrolState.PURSUING:
                //Change the speed
                agent.speed = runSpeed;
                // chases after the target
                agent.SetDestination(target.transform.position);
                //Changes the animation depending on the speed the agent is moving
                anim.SetFloat("BlendX", agent.velocity.x);
                anim.SetFloat("BlendY", agent.velocity.z);
                //If they hide while we are pursuing them
                if (!canSee && !canHear)
                {
                    if (playersState == PlayerState.HIDE)
                    {  //change state to search then start coroutine
                        search = true;
                        _patrolState = PatrolState.SEARCH;
                        StartCoroutine(CountDown(vigilantTime, alerted));
                    }
                }
                break;

            //State triggered after being alerted, fov and hearing radius increase
            case PatrolState.VIGILANT:
                //Set the fov
                fovAngle = vigilantFov;
                //Changes the animation depending on the speed the agent is moving
                anim.SetFloat("BlendX", agent.velocity.x);
                anim.SetFloat("BlendY", agent.velocity.z);
                //increase the radius of the sphere collider trigger
                hearingRadius.radius = vigilantRad;
                //if patrol can see or hear player then pursue them
                if (canSee || canHear)
                {
                    _patrolState = PatrolState.PURSUING;
                }
                //if we aren't alerted anymore go back to patrolling 
                if (!alerted) { _patrolState = PatrolState.PATROLLING; }
                //move the patrol 
                if (agent.remainingDistance <= 1 || agent.destination == null)
                { getNewDestination(); }
                break;

            //Patrol "Wanders" randomly around inside of a navSphere
            case PatrolState.WANDER:
                if (!wander) { _patrolState = PatrolState.PATROLLING; }
                //Set the fov
                fovAngle = normalFov;
                //Change the speed
                agent.speed = walkSpeed;
                //Changes the animation depending on the speed the agent is moving
                anim.SetFloat("BlendX", agent.velocity.x);
                anim.SetFloat("BlendY", agent.velocity.z);
                //set sphere collider radius back to normal
                hearingRadius.radius = normalRad;
                //move the patrol 
                if (agent.remainingDistance <= 1 || agent.destination == null || agent.velocity.magnitude == 0)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderDistance, 9);
                    agent.SetDestination(newPos);
                }
                //Swap to vigilant
                if (alerted)
                {
                    //change state to alerted then start coroutine
                    _patrolState = PatrolState.VIGILANT;
                    StartCoroutine(CountDown(vigilantTime, alerted));
                }
                break;

            //Similar to wander, but the wander area is smaller and the patrol's speed is slower
            case PatrolState.SEARCH:
                //half the speed of walking 
                agent.speed = walkSpeed / 2;
                //Changes the animation depending on the speed the agent is moving
                anim.SetFloat("BlendX", agent.velocity.x);
                anim.SetFloat("BlendY", agent.velocity.z);
                //move the patrol 
                if (agent.remainingDistance <= 1 || agent.velocity.magnitude == 0)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderDistance / 4, 9);
                    agent.SetDestination(newPos);
                }
                if (canHear || canSee) { _patrolState = PatrolState.PURSUING; }
                if (!search) { _patrolState = PatrolState.PATROLLING; }
                break;

            case PatrolState.STUNNED:
                //Set the fov
                fovAngle = 0;
                //Change the speed
                agent.speed = 0;
                //change the radius
                hearingRadius.radius = 0;
                //Changes the animation depending on the speed the agent is moving
                anim.SetFloat("BlendX", agent.velocity.x);
                anim.SetFloat("BlendY", agent.velocity.z);
                if (!stunned) { _patrolState = PatrolState.PATROLLING; }
                break;

            default:
                break;
        }
    }

    //This might need to change
    //The logic with searching and vigilant is a little weird at the moment
    IEnumerator CountDown(float time, bool boolToChange)
    {
        yield return new WaitForSeconds(time);
        alerted = false;
        search = false;
        StopCoroutine(CountDown(time, boolToChange));
    }

    IEnumerator Stunned(float time) {
        stunned = true;
        yield return new WaitForSeconds(time);
        stunned = false;
        StopCoroutine(Stunned(time));
    }


    //Used to make it so patrol can't hear player through walls
    bool CanHearPlayer(Vector3 dirToTarget)
    {
        //check if something is blocking hearing of the patrol
        if (!Physics.Linecast(transform.position, target.transform.position, viewMask))
        {
            //draw a line between the patrol and the player
            //Debug.DrawRay(transform.position, directionTotarget, Color.yellow);
            //alerted = true;
            //can hear player
            alerted = true;
            return true;
        }
        return false;
    }


    bool CanSeePlayer(Vector3 dirToTarget)
    {
        //if player is within the view distance
        if (distance < viewDistance)
        {
            //returns the smallest angle between the two
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToTarget);
            //Debug.Log(angleBetweenGuardAndPlayer);
            //check if the angle between patrol's forward direction and the direction to player is within the view angle
            if (angleBetweenGuardAndPlayer < fovAngle / 2)
            {

                //check if line of sight of the guard is blocked
                if (!Physics.Linecast(transform.position, target.transform.position, viewMask))
                {
                    //draw a line between the patrol and the player
                    //Debug.DrawRay(transform.position, dirToTarget, Color.red, 2f, false);
                    //can see player
                    return true;
                }
            }
        }
        //can't see player 
        return false;
    }

    void playerHide()
    {
        playerHidden = !playerHidden;
    }

    void playerState(PlayerState state)
    {
        //Debug.Log(playersState);
        playersState = state;
    }

    void stunPatrol(float stunTime) {
        if (distance <=4) {
            StartCoroutine(Stunned(stunTime));
            _patrolState = PatrolState.STUNNED;
        }
    }

    /*
    //Shows the patrol's field of view
    private void OnDrawGizmos()
    {
        float totalFOV = fovAngle;
        float rayRange = viewDistance;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);
    }
    */

}
