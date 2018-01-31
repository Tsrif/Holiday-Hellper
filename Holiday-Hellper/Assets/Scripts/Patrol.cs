using System.Collections;
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
    private NavMeshAgent agent;
    private Vector3 directionTotarget;
    private SphereCollider sphereCollider;
    public float distance;

    public float normalRad;
    public float vigilantRad;

    public float vigilantTime = 5f;

    public bool alerted; //Used to swap us between patrol and vigilant 
                          //Alerted triggered after pursuing state set 
                          //Uses coroutine to turn alerted off
    

    public PatrolState _patrolState;

    private GameState gameState = GameState.PLAYING;

    // Use this for initialization
    private void Start()
    {
        _patrolState = PatrolState.PATROLLING;
        wanderIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
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
        //Calculate the distance between the patrol and the player 
        distance = Vector3.Distance(transform.position, target.transform.position);

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

        //Swap between states
        changeState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            if (other.gameObject.GetComponent<PlayerController>().soundRadius)
            {
                _patrolState = PatrolState.PURSUING;
            }

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            directionTotarget = other.transform.position - transform.position;
            targetAngle = Vector3.Angle(directionTotarget, transform.forward);

            if (targetAngle < fovAngle * 0.5f)
            {
                  Debug.DrawRay(transform.position, directionTotarget, Color.yellow);
                if (Physics.Raycast(transform.position, directionTotarget, out hit, 100, LayerMask.NameToLayer("Everything"), QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.gameObject == target)
                    {
                        _patrolState = PatrolState.PURSUING;
                    }
                }
            }
        }
    }

    // if the target gets out of range while chasing them
    // go back to wandering/patrolling
    private void OnTriggerExit(Collider other)
    {
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

    void changeState()
    {
        switch (_patrolState)
        {
            case PatrolState.PATROLLING:
                //set sphere collider radius back to normal
                sphereCollider.radius = normalRad;
                //move the patrol 
                if (agent.remainingDistance <= 2 || agent.destination == null)
                { getNewDestination(); }
                if (alerted) {
                    //change state to alerted then start coroutine
                    _patrolState = PatrolState.VIGILANT;
                    StartCoroutine(CountDown());
                }
                break;
            case PatrolState.PURSUING:
                // chases after the target
                agent.SetDestination(target.transform.position);
                //set alerted to true
                alerted = true;
                //if we get too close and the player is hiding then swap back to patrolling
                if (distance < 1 && target.GetComponent<PlayerController>().hide)
                { _patrolState = PatrolState.PATROLLING; }
                break;
            case PatrolState.VIGILANT:
                //increase the radius of the sphere collider trigger
                sphereCollider.radius = vigilantRad;
                //if we aren't alerted anyomre go back to patrolling 
                if (!alerted) { _patrolState = PatrolState.PATROLLING; }
                //move the patrol 
                if (agent.remainingDistance <= 2 || agent.destination == null)
                { getNewDestination(); }

                break;
            default:
                break;
        }
    }

    IEnumerator CountDown() {
        yield return new WaitForSeconds(vigilantTime);
        alerted = false;
        StopCoroutine(CountDown());
    }
}
