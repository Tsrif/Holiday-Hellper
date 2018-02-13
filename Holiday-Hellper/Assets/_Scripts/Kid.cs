using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]

public class Kid : MonoBehaviour {

    /*
     * 
     * Script to use for making the kid wandering around like the patrol units
     * almost the same but the chase/pursue functionality has been disabled.
     * Currently is not needed until later 
     * 
     */

    public bool isPatrolling;    

    [SpaceAttribute]

    public Transform[] patrolPoints;    

    private int wanderIndex;
    private NavMeshAgent agent;
    private SphereCollider sphereCollider;
    public cakeslice.Outline outline;

    private GameState gameState = GameState.PLAYING;

	// Use this for initialization
	private void Start () {
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
    // Update is called once per frame
    void Update ()
    {
        //if (gameState == GameState.PAUSED || gameState == GameState.WIN)
        //{
        //    agent.isStopped = true;
        //    return;
        //}
        //else
        //{           
        //    agent.isStopped = false;
        //}

        //if (agent.remainingDistance <= 2)
        //{
        //    getNewDestination();
        //}
	}

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            outline.color = 2;
        }
    }

    // if the target gets out of range while chasing them
    // go back to wandering/patrolling
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            outline.color = 1;
        }

    }

    // cycle through series of points
    // to wander around a set area
    private void Wander(){

    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void getNewDestination() {

        wanderIndex++;

        if (wanderIndex >= patrolPoints.Length)
        {
            wanderIndex = 0;
        }

        agent.SetDestination(patrolPoints[wanderIndex].transform.position);
    }
}
