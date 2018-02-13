using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Doors : Interactable {
    public bool open;
    public bool locked;
    public Animator doorAnimator;

    public void Start()
    {
        open = false;
        locked = false;
        doThis = openDoors;
        doorAnimator = GetComponent<Animator>();
    }

    public void openDoors() {

        if (open == false && locked == false)
        {
            Debug.Log("Did this again");
            doorAnimator.SetBool("open", true);
            open = true;
            GetComponent<NavMeshObstacle>().carving = false;
        }
        else if (open == true) {
            Debug.Log("Got Here");
            doorAnimator.SetBool("open", false);
            open = false;
            GetComponent<NavMeshObstacle>().carving = true;
        }
    }
}
