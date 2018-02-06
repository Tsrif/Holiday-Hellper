using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//Patrol - Walks between set points
//Pursuing - Chases player
//Similar to Patrol, but now radius to notice player is much larger 
public enum PatrolState { PATROLLING, PURSUING, VIGILANT };

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
    private float targetAngle;
    private RaycastHit hit;
    public NavMeshAgent agent;
    private Vector3 directionTotarget;
    public SphereCollider hearingRadius;
    public float distance;

    public float walkSpeed;
    public float runSpeed;

    public Animator anim;

    public float normalRad;
    public float vigilantRad;

    public float viewDistance;

    public float normalFov;
    public float vigilantFov;

    public float vigilantTime = 5f;

    public bool alerted; //Used to swap us between patrol and vigilant 
                         //Alerted triggered after pursuing state set 
                         //Uses coroutine to turn alerted off


    public bool canHear;
    public bool canSee;

    public LayerMask viewMask;




    public PatrolState _patrolState;

    private GameState gameState = GameState.PLAYING;


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
    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
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
        if (target.GetComponent<PlayerController>()._playerState == PlayerState.HIDE)
        {
            canHear = false;
            canSee = false;
        }

        //if patrol can see or hear player then pursue them
        if (canSee || canHear)
        {
            _patrolState = PatrolState.PURSUING;
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
            //if the player enters our hearing field we can only hear them if they aren't sneaking
            if (target.GetComponent<PlayerController>()._playerState == PlayerState.SNEAK)
            {
                print("He enter but he sneak");
                directionTotarget = other.transform.position - transform.position;
                canSee = CanSeePlayer(directionTotarget);
                return;
            }

            directionTotarget = other.transform.position - transform.position;
            canSee = CanSeePlayer(directionTotarget);
            canHear = true;
            alerted = true;
            Debug.DrawRay(transform.position, directionTotarget, Color.yellow);

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
                _patrolState = PatrolState.PATROLLING;

            }
        }
    }

    // cycle through series of points
    // to wander around a set area
    private void Wander()
    {

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

    //Change states and control what is done in those states
    void changeState()
    {
        switch (_patrolState)
        {
            case PatrolState.PATROLLING:
                //Set the fov
                fovAngle = normalFov;
                //Change the speed
                agent.speed = walkSpeed;
                //set sphere collider radius back to normal
                hearingRadius.radius = normalRad;
                //move the patrol 
                if (agent.remainingDistance <= 1 || agent.destination == null)
                { getNewDestination(); }
                if (alerted)
                {
                    //change state to alerted then start coroutine
                    _patrolState = PatrolState.VIGILANT;
                    StartCoroutine(CountDown());
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
                //set alerted to true
                alerted = true;
                //if we get too close and the player is hiding then swap back to patrolling
                if (distance < 1 && target.GetComponent<PlayerController>().hide)
                { _patrolState = PatrolState.PATROLLING; }
                break;

            case PatrolState.VIGILANT:
                //Set the fov
                fovAngle = vigilantFov;
                //increase the radius of the sphere collider trigger
                hearingRadius.radius = vigilantRad;
                //if we aren't alerted anyomre go back to patrolling 
                if (!alerted) { _patrolState = PatrolState.PATROLLING; }
                //move the patrol 
                if (agent.remainingDistance <= 1 || agent.destination == null)
                { getNewDestination(); }
                break;

            default:
                break;
        }
    }

    IEnumerator CountDown()
    {
        yield return new WaitForSeconds(vigilantTime);
        alerted = false;
        StopCoroutine(CountDown());
    }


    bool CanSeePlayer(Vector3 dirToTarget)
    {
        directionTotarget = dirToTarget;
        //if player is within the view distance
        if (distance < viewDistance)
        {
            //returns the smallest angle between the two
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, directionTotarget);
            //check if the angle between patrol's forward direction and the direction to player is within the view angle
            if (angleBetweenGuardAndPlayer < fovAngle / 2f)
            {
                //check if line of sight of the guard is blocked
                if (!Physics.Linecast(transform.position, target.transform.position, viewMask))
                {
                    //draw a line between the patrol and the player
                    Debug.DrawRay(transform.position, directionTotarget, Color.red, 2f, false);
                    //can see player
                    return true;
                }
            }
        }
        //can't see player 
        return false;
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
