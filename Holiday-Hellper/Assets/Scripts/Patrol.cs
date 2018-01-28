using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]

public class Patrol : MonoBehaviour {

    public bool isPursuing;
    public bool isPatrolling;    

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

    private GameState gameState = GameState.PLAYING;

	// Use this for initialization
	private void Start () {
        wanderIndex = 0;		
        agent = GetComponent<NavMeshAgent>();
        sphereCollider = GetComponent<SphereCollider>();
        isPatrolling = true;
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
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (gameState == GameState.PAUSED || gameState == GameState.WIN)
        {
            agent.isStopped = true;
            return;
        }
        else
        {            
            agent.isStopped = false;
            //Debug.Log("Dest: " + agent.destination);
        }

        //Debug.Log(gameObject.name + " : " + agent.remainingDistance);

        if(isPatrolling) {

            if (agent.remainingDistance <= 2 || agent.destination == null )
            {
              getNewDestination();
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
              //  Debug.DrawRay(transform.position, directionTotarget, Color.yellow);
                if(Physics.Raycast(transform.position, directionTotarget, out hit, 100, LayerMask.NameToLayer("Everything"), QueryTriggerInteraction.Ignore)){

                   // Debug.Log("Name: " + hit.transform.name);

                    if(hit.transform.gameObject == target){
                        isPursuing = true;  
                        Pursue();
                    }
                }

                /* Sunday Logic
                wall = false;
                targetLocated = false;

                for (int i = 0; i < checkForPlayer.Length; i++)
                {
                    if (checkForPlayer[i].collider.tag == "wall")
                    {
                        wall = true;
                    }else if (checkForPlayer[i].collider.tag == "Player") {
                        targetLocated = true;
                    }
                }

                if (wall == false && targetLocated == true)
                {
                    Pursue();
                }
                */
            }else{
                isPatrolling = true;
            }
        }
    }

    // if the target gets out of range while chasing them
    // go back to wandering/patrolling
    private void OnTriggerExit(Collider other)
    {
        if(isPursuing){

            if (other.gameObject == target){
                isPatrolling = true;
                isPursuing = false;

            }
        }
    }

    // cycle through series of points
    // to wander around a set area
    private void Wander(){

    }

    // chases after the target
    private void Pursue(){

        isPatrolling = false;
        isPursuing = true;
        
        agent.SetDestination(target.transform.position);
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
