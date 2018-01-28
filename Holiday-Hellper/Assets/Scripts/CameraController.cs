using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform target;

    public Vector3 offset;
    public bool useOffsetValues;

    public float rotateSpeed;
    public Transform pivot;
    public bool invertY;

    public float maxView;
    public float minView;
    public bool hide;


    private GameState gameState = GameState.PLAYING;
    // Use this for initialization
    void Start() {

        Cursor.lockState = CursorLockMode.Locked;

        if (!useOffsetValues)
        {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

    }

    private void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        PlayerAbilities.hide += playerHide;
    }
    private void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        PlayerAbilities.hide -= playerHide;
    }

    // Update is called once per frame
    void LateUpdate() {

        if (gameState == GameState.PAUSED || gameState == GameState.WIN || hide)
        {
            return;
        }


        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera.main.fieldOfView > 20)
            {
                Camera.main.fieldOfView--;
            }

        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (Camera.main.fieldOfView < 50)
            {
                Camera.main.fieldOfView++;
            }

        }
        //Getting mouse X pos and rotate the target
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        target.Rotate(0, horizontal, 0);
        //Getting mouse Y pos and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

        if (invertY)
        {
            pivot.Rotate(vertical, 0, 0);
        }
        else
        {
            pivot.Rotate(-vertical, 0, 0);
        }
        //limit up/down camera rotation
        /*
        if(pivot.rotation.eulerAngles.x > maxView && pivot.rotation.eulerAngles.x < 180f)
        {
            pivot.rotation = Quaternion.Euler(maxView, 0, 0);
        }

        if(pivot.rotation.eulerAngles.x > 180 && pivot.rotation.eulerAngles.x < 360f + minView)
        {
            pivot.rotation = Quaternion.Euler(360f + minView, 0, 0);
        }
        */

        //Move camera based on target rotation and offset
        float yAngle = target.eulerAngles.y;
        //float xAngle = pivot.eulerAngles.x;

        Quaternion rotation = Quaternion.Euler(0, yAngle, 0); //xAngle
        transform.position = target.position - (rotation * offset);

        //transform.position = target.position - offset;

        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
        }
        transform.LookAt(target);


    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void playerHide() {
        hide = !hide;
    }
}
