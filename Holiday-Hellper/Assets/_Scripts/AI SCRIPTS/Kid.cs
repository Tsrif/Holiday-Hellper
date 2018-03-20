using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public enum KidState { SLEEPING, FLEEING, TERRIFIED, WALKING, SCREAMING, FIGHTING, STUNNED };

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(SphereCollider))]

/* This class defines the basic behavior of the Kid AI
 * All of the child classes of this will only use a few specific states and 
 * the transistions between the states will be handled in those respective classes
 */

public class Kid : MonoBehaviour
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

    public float viewDistance;

    public float normalFov;
    public float wanderDistance;
    [SpaceAttribute]
    public bool alerted; //Used to swap us between patrol and vigilant 
                         //Alerted triggered after pursuing state set 
                         //Uses coroutine to turn alerted off
    public bool stunned;
    public bool canHear;
    public bool canSee;

    public bool wander;
    public bool inside;

    public LayerMask viewMask;

    private bool playerHidden;

    private PlayerState playersState;

    public GameObject player;

    public KidState _kidState;

    private GameState gameState = GameState.PLAYING;
    [SpaceAttribute]

    public Vector3 offsetVector;
    public float reactionTime;

    private int sleepId = Animator.StringToHash("Sleep");

    public static event Action<bool> spottedPlayer;


    // Use this for initialization
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _kidState = KidState.SLEEPING;
        wanderIndex = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.avoidancePriority = UnityEngine.Random.Range(1, 100); //set random priority
        hearingRadius = GetComponent<SphereCollider>();
        alerted = false;
        agent.SetDestination(patrolPoints[0].position);
        StartCoroutine(ReactionDelay(reactionTime));
        agent.speed = 0;
    }

    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        PlayerController.CurrentState += playerState;
        Stun.stun += stunKid;
    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        PlayerController.CurrentState -= playerState;
        Stun.stun -= stunKid;
    }

    void Update()
    {

        DefaultUpdate();
    }

    //Behavior to be used in the update function 
    void DefaultUpdate()
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

        if (target == null) { target = player; inside = canSee = canHear = false; }//after the decoy is destroyed the current target is set back to the Player and values are reset back to false

        //Calculate the distance between the patrol and the player 
        distance = Vector3.Distance(transform.position, target.transform.position);

        //If the player is hiding then patrol can't see or hear them 
        if (playersState == PlayerState.HIDE)
        {
            canHear = false;
            canSee = false;
        }

        //This kind of a bad way to send this notification, but I wasn't sure where else to put it / how
        if (canSee || canHear)
        {
            if (spottedPlayer != null)
            {
                spottedPlayer(true);
            }
        }
        else
        {
            if (spottedPlayer != null)
            {
                spottedPlayer(false);
            }
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
            inside = true;
            directionTotarget = ((target.transform.position + offsetVector) - transform.position);
        }
    }

    // if the target gets out of range while chasing them
    // go back to wandering/patrolling
    private void OnTriggerExit(Collider other)
    {
        inside = false;
        canHear = false;
        //if (_patrolState == PatrolState.PURSUING)
        //{

        //    if (other.gameObject == target)
        //    {
        //        alerted = true;
        //        _patrolState = PatrolState.PATROLLING;

        //    }
        //}
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

        Vector3 finalPosition = Vector3.zero;
        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask))
        {
            finalPosition = navHit.position;
        }
        return finalPosition;
    }

    //Change states and control what is done in those states
    void changeState()
    {
        //SLEEPING, FLEEING, TERRIFIED, WALKING, SCREAMING, FIGHTING, STUNNED 

        switch (_kidState)
        {
            //Sleep, kid doesn't move
            case KidState.SLEEPING:
                //change the speed to 0
                agent.speed = 0;
                // change to the sleeping animation
                anim.SetInteger("State", (int)_kidState);
                //if (canHear) { anim.SetTrigger("StandUp"); _kidState = KidState.FLEEING;}
                break;

            // Runs away from player 
            case KidState.FLEEING:
                if (anim.GetCurrentAnimatorStateInfo(0).shortNameHash == sleepId)
                {
                    print("He sleep");
                    anim.SetTrigger("StandUp");
                    //Do something if this particular state is palying
                }
                agent.speed = runSpeed;
                // change to the sleeping animation
                anim.SetInteger("State", (int)_kidState);
                //move the kid 
                if (agent.remainingDistance <= 1 || agent.destination == null || agent.velocity.magnitude == 0)
                {
                    Vector3 newPos = RandomNavSphere(transform.position, wanderDistance, 9);
                    agent.SetDestination(newPos);
                }
                break;

            // Cowers in fear
            case KidState.TERRIFIED:
                agent.speed = 0;
                // change to the sleeping animation
                anim.SetInteger("State", (int)_kidState);
                break;

            // Walks Between Points 
            case KidState.WALKING:
                agent.speed = walkSpeed;
                // change the animation
                anim.SetInteger("State", (int)_kidState);
                //move the kid 
                if (agent.remainingDistance <= 1 || agent.destination == null)
                { getNewDestination(); }
                break;

            case KidState.SCREAMING:
                break;

            case KidState.FIGHTING:
                break;

            case KidState.STUNNED:
                //change the speed to 0
                agent.speed = 0;
                // change to the stunned animation
                anim.SetInteger("State", (int)_kidState);
                break;

        }

    }

    IEnumerator Stunned(float time)
    {
        stunned = true;
        yield return new WaitForSeconds(time);
        stunned = false;
        StopCoroutine(Stunned(time));
    }

    IEnumerator ReactionDelay(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            if (playersState == PlayerState.SNEAK)
            {
                canSee = CanSeePlayer(directionTotarget);
            }
            else
            {
                canSee = CanSeePlayer(directionTotarget);
                canHear = CanHearPlayer(directionTotarget);
            }

        }
    }

    //Used to make it so kid can't hear player through walls
    bool CanHearPlayer(Vector3 dirToTarget)
    {
        //If the player isn't inside return false 
        if (!inside)
        {
            return false;
        }
        //check if something is blocking hearing of the patrol
        if (!Physics.Linecast(transform.position, target.transform.position, viewMask))
        {
            return true;
        }
        return false;
    }

    bool CanSeePlayer(Vector3 dirToTarget)
    {
        //If the player isn't inside return false 
        if (!inside) { return false; }
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

    void stunKid(float stunTime)
    {
        if (distance <= 4)
        {
            StartCoroutine(Stunned(stunTime));
            _kidState = KidState.STUNNED;
        }
    }
}
