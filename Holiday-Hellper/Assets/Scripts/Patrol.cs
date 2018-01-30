using UnityEngine;
using UnityEngine.AI;

public enum PatrolState { PATROLLING, PURSUING };

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
    private bool wall = false;
    private bool targetLocated = false;
    public float distance;

    public PatrolState _patrolState;

    private GameState gameState = GameState.PLAYING;

    // Use this for initialization
    private void Start()
    {
        _patrolState = PatrolState.PATROLLING;
        wanderIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();

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

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target)
        {
            directionTotarget = other.transform.position - transform.position;
            targetAngle = Vector3.Angle(directionTotarget, transform.forward);

            if (targetAngle < fovAngle * 0.5f)
            {
                //  Debug.DrawRay(transform.position, directionTotarget, Color.yellow);
                if (Physics.Raycast(transform.position, directionTotarget, out hit, 100, LayerMask.NameToLayer("Everything"), QueryTriggerInteraction.Ignore))
                {
                    if (hit.transform.gameObject == target)
                    {
                        _patrolState = PatrolState.PURSUING;
                    }
                }
            }
            else
            {
                _patrolState = PatrolState.PATROLLING;
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
                if (agent.remainingDistance <= 2 || agent.destination == null)
                { getNewDestination(); }
                break;
            case PatrolState.PURSUING:
                // chases after the target
                agent.SetDestination(target.transform.position);
                //if we get too close and the player is hiding then swap back to patrolling
                if (distance < 1 && target.GetComponent<PlayerController>().hide == true)
                {
                    _patrolState = PatrolState.PATROLLING;
                }
                break;
            default:
                break;
        }
    }
}
