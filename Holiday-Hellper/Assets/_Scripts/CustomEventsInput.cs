using UnityEngine;
using UnityEngine.EventSystems;

public class CustomEventsInput : MonoBehaviour
{
    private GameObject currentButton;
    private AxisEventData currentAxis;
    //timer
    public float timeBetweenInputs = 0.15f; //in seconds
    [Range(0, 1)]
    public float deadZone = 0.15f;
    private float timer = 0;

    void Update()
    {
        if (timer <= 0)
        {
            currentAxis = new AxisEventData(EventSystem.current);
            currentButton = EventSystem.current.currentSelectedGameObject;
            if (currentButton != null)
            {
                ///Debug.Log(currentButton.name);
            }
            if (Input.GetAxis("Gamepad Left Vertical") > deadZone) // move up
            {
                //Debug.Log("Move Up");
                currentAxis.moveDir = MoveDirection.Up;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            }
            else if (Input.GetAxis("Gamepad Left Vertical") < -deadZone) // move down
            {
                //Debug.Log("Move Down");
                currentAxis.moveDir = MoveDirection.Down;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            }
            else if (Input.GetAxis("Gamepad Left Horizontal") > deadZone) // move right
            {
                //Debug.Log("Move Right");
                currentAxis.moveDir = MoveDirection.Right;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            }
            else if (Input.GetAxis("Gamepad Left Horizontal") < -deadZone) // move left
            {
                //Debug.Log("Move Left");
                currentAxis.moveDir = MoveDirection.Left;
                ExecuteEvents.Execute(currentButton, currentAxis, ExecuteEvents.moveHandler);
            }
            timer = timeBetweenInputs;
        }

        //timer counting down
        timer -= Time.fixedDeltaTime;

    }
}