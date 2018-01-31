using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerState { IDLE, WALKING, SNEAK, RUNNING, HIDE, CARRYING };
public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float walkSpeed;
    public float sneakSpeed;
    public float runSpeed;
    public float jumpForce;

    public CharacterController controller;
    public Vector3 moveDirection;
    public float gravityScale;

    public int jumpCount;
    public Animator anim;

    private GameState gameState = GameState.PLAYING;
    private Interact interact;

    public bool hide;

    public SphereCollider soundRadius;
    public PlayerState _playerState;

    //All the radiuses the soundRadius can be 
    public float idleRad;
    public float walkRad;
    public float sneakRad;
    public float runningRad;
    public float hideRad;
    public float carryingRad;

    // Use this for initialization
    void Start()
    {

        controller = GetComponent<CharacterController>();
        interact = GetComponent<Interact>();
        _playerState = PlayerState.IDLE;
    }


    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        //PlayerAbilities.hide += playerHide;
        Hide.hide += playerHide;
        hide = false;
        _playerState = PlayerState.IDLE;

    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        //PlayerAbilities.hide -= playerHide;
        Hide.hide -= playerHide;
    }
    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.PAUSED || gameState == GameState.WIN)
        {
            return;
        }
        ChangeState(); 
    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void playerHide()
    {
        hide = !hide;
        _playerState = PlayerState.HIDE;
    }

    void ChangeState()
    {
        switch (_playerState)
        {
            case PlayerState.IDLE:
                Movement();
                if (Input.GetAxis("Sneak") > 0) { _playerState = PlayerState.SNEAK; }
                if (Input.GetAxis("Run") > 0) { _playerState = PlayerState.RUNNING; }
                if (Input.GetAxis("Vertical") != 0|| Input.GetAxis("Horizontal") != 0) { _playerState = PlayerState.WALKING; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                anim.SetBool("Walk", true);
                soundRadius.radius = idleRad;
                break;

            case PlayerState.WALKING:
                Movement();
                if (Input.GetAxis("Sneak") > 0) { _playerState = PlayerState.SNEAK; }
                if (Input.GetAxis("Run") > 0) { _playerState = PlayerState.RUNNING; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                if (getMoveDir() == 0) { _playerState = PlayerState.IDLE; }
                moveSpeed = walkSpeed;
                soundRadius.radius = walkRad;
                break;

            case PlayerState.SNEAK:
                Movement();
                if (Input.GetAxis("Sneak") == 0) { _playerState = PlayerState.IDLE; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                moveSpeed = sneakSpeed;
                soundRadius.radius = sneakRad;
                break;

            case PlayerState.RUNNING:
                Movement();
                if (Input.GetAxis("Sneak") > 0) { _playerState = PlayerState.SNEAK; }
                if (Input.GetAxis("Run") == 0) { _playerState = PlayerState.IDLE; }
                if (interact.carrying) { _playerState = PlayerState.CARRYING; }
                soundRadius.radius = runningRad;
                moveSpeed = runSpeed;
                break;

            case PlayerState.HIDE:
                if (!hide) { _playerState = PlayerState.IDLE; }
                soundRadius.radius = hideRad;
                break;

            case PlayerState.CARRYING:
                Movement();
                if (!interact.carrying) { _playerState = PlayerState.IDLE; }
                anim.SetBool("Walk", false);
                moveSpeed = sneakSpeed;
                soundRadius.radius = carryingRad;
                break;

            default:
                break;
        }
    }

    //Helper function to get the movement direction
    //Ignores Y
    float getMoveDir() {
        float temp1 = moveDirection.x;
        float temp2 = moveDirection.z;
        return temp1 + temp2;

    }

    void Movement() {
        float y = moveDirection.y;
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = y;
        anim.SetFloat("BlendX", controller.velocity.x);
        anim.SetFloat("BlendY", controller.velocity.z);
        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }

}
