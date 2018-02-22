using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum PlayerState { IDLE, WALKING, SNEAK, RUNNING, HIDE, CARRYING, WALKING_TO_SNEAK, SNEAK_TO_WALKING,WALK_TO_IDLE };
public enum PlayerVisibility { NOTVISIBLE, VISIBLE };
public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float walkSpeed;
    public float sneakSpeed;
    public float runSpeed;
    [SpaceAttribute]
    public CharacterController controller;
    public Vector3 moveDirection;
    public float gravityScale;

    public int jumpCount;
    public Animator anim;

    private GameState gameState = GameState.PLAYING;
    private Interact interact;

    public HideState hide;

    public SphereCollider soundRadius;
    public PlayerState _playerState;
    public PlayerVisibility _playerVisibility;

    //All the radiuses the soundRadius can be 
    public float idleRad;
    public float walkRad;
    public float sneakRad;
    public float runningRad;
    public float hideRad;
    public float carryingRad;

    public Transform pivot;
    public float rotateSpeed;
    public GameObject playerModel;
    [SpaceAttribute]

    //All the input 
    public float verticalInput;
    public float horizontalInput;
    public float sneakInput;
    public float runInput;

    public static event Action<PlayerState> CurrentState;

    void Start()
    {
        _playerVisibility = PlayerVisibility.NOTVISIBLE;
        controller = GetComponent<CharacterController>();
        interact = GetComponent<Interact>();
        _playerState = PlayerState.IDLE;
    }


    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        Hide.hide += playerHide;
        hide = HideState.NOT_HIDDEN;
        anim.SetBool("Walk", true);
        _playerState = PlayerState.IDLE;

    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        Hide.hide -= playerHide;
    }

    void Update()
    {
        if (gameState == GameState.PAUSED || gameState == GameState.WIN )
        {
            return;
        }
        GetInput();
        ChangeState();
    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void playerHide(HideState state)
    {
        hide = state;
        if (hide == HideState.HIDDEN) { _playerState = PlayerState.HIDE; }
    }

    void ChangeState()
    {
        switch (_playerState)
        {
            case PlayerState.IDLE:
                Movement(0);
                if (sneakInput> 0) {_playerState = PlayerState.WALKING_TO_SNEAK;}
                if (runInput > 0) { _playerState = PlayerState.RUNNING; }
                if (verticalInput != 0|| horizontalInput != 0) { _playerState = PlayerState.WALKING; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                anim.SetBool("Walk", true);
                soundRadius.radius = idleRad;
                break;

            case PlayerState.WALKING:
                Movement(walkSpeed);
                if (sneakInput > 0) { _playerState = PlayerState.WALKING_TO_SNEAK; }
                if (runInput > 0) { _playerState = PlayerState.RUNNING; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                if (getMoveDir() == 0) { _playerState = PlayerState.IDLE; }
                soundRadius.radius = walkRad;
                break;

            case PlayerState.SNEAK:
                Movement(sneakSpeed);
                if (sneakInput == 0) { _playerState = PlayerState.SNEAK_TO_WALKING; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                soundRadius.radius = sneakRad;
                break;

            case PlayerState.RUNNING:
                Movement(runSpeed);
                if (sneakInput > 0) { _playerState = PlayerState.SNEAK; }
                if (runInput == 0) { _playerState = PlayerState.IDLE; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                soundRadius.radius = runningRad;
                break;

            case PlayerState.HIDE:
                if (hide == HideState.NOT_HIDDEN) { _playerState = PlayerState.IDLE; }
                soundRadius.radius = hideRad;
                break;

            case PlayerState.CARRYING:
                Movement(sneakSpeed);
                if (!interact.carrying) { _playerState = PlayerState.IDLE; }
                anim.SetBool("Walk", false);
                soundRadius.radius = carryingRad;
                break;

            case PlayerState.WALKING_TO_SNEAK:
                //Turn off walk
                anim.SetBool("Walk", false);
                //Transition
                anim.SetTrigger("Change To Sneak");
                //turn on sneak
                anim.SetBool("Sneak", true);
                //change state
                _playerState = PlayerState.SNEAK;
                break;

            case PlayerState.SNEAK_TO_WALKING:
                //Turn on walk
                anim.SetBool("Walk", true);
                //Transition
                anim.SetTrigger("Change To Walk");
                //turn off sneak
                anim.SetBool("Sneak", false);
                //change state
                _playerState = PlayerState.WALKING;
                break;

            case PlayerState.WALK_TO_IDLE:
                anim.SetBool("Walk", false);
                anim.SetTrigger("Stop Walk");
                _playerState = PlayerState.IDLE;
                break;

            default:
                break;
        }
        if (CurrentState != null) { CurrentState(_playerState);}
    }

    void GetInput() {
        //Horizontal
        horizontalInput = Input.GetAxis("Horizontal");
        // vertical
        verticalInput = Input.GetAxis("Vertical");
        //sneak 
        sneakInput = Input.GetAxis("Sneak");
        // run 
        runInput = Input.GetAxis("Run");
        // punch 
    }

    //Helper function to get the movement direction
    //Ignores Y
    float getMoveDir() {
        float temp1 = moveDirection.x;
        float temp2 = moveDirection.z;
        return temp1 + temp2;
    }

    void Movement(float moveSpeed) {
        float y = moveDirection.y;
        moveDirection = (transform.forward * verticalInput) + (transform.right * horizontalInput);
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = y;
        anim.SetFloat("BlendX", controller.velocity.x);
        anim.SetFloat("BlendY", controller.velocity.z);
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
        //move player in different directions based on camera direction
        if (horizontalInput != 0 || verticalInput != 0)
        {
            transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }
    }

}
