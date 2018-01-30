using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed;
	public float walkSpeed;
	public float sneakSpeed;
    public float jumpForce;

    public CharacterController controller;
    private Vector3 moveDirection;
    public float gravityScale;

    public int jumpCount;
    public Animator anim;

    private GameState gameState = GameState.PLAYING;
	private Interact  interact;

    public bool hide;

    // Use this for initialization
    void Start () {

        controller = GetComponent<CharacterController>();
		interact = GetComponent<Interact>();
	}


    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        //PlayerAbilities.hide += playerHide;
        Hide.hide += playerHide;
        hide = false;

    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        //PlayerAbilities.hide -= playerHide;
        Hide.hide -= playerHide;
    }
    // Update is called once per frame
    void Update () {


        if(gameState == GameState.PAUSED || gameState == GameState.WIN )
        {
            return;
        }

        float y = moveDirection.y;

        //Debug.Log("Value: " + Input.GetAxis("Sneak"));
		if (Input.GetAxis("Sneak") > 0 || interact.carrying == true)
        {
			moveSpeed = sneakSpeed;

		} else
        {
			moveSpeed = walkSpeed;
		}
		if (interact.carrying == true) {
			anim.SetBool("Walk",false);
			
		} else {
			anim.SetBool("Walk",true);
		}

        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection.y = y;
        anim.SetFloat("BlendX", controller.velocity.x);
        anim.SetFloat("BlendY", controller.velocity.z);
		//print ("Controller vel x " +  controller.velocity.x);



        //Jump
        if (controller.isGrounded)
        {

            jumpCount = 0;
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }

        }

        //Double jump
        if(!controller.isGrounded)
        {              
            if(Input.GetButtonDown("Jump"))
            {
                if(jumpCount == 0)
                {
                    moveDirection.y = jumpForce / 1.5f;
                    jumpCount += 1;
                }                 
            }              
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);
        controller.Move(moveDirection * Time.deltaTime);
    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void playerHide()
    {
        hide = !hide;
    }
}
