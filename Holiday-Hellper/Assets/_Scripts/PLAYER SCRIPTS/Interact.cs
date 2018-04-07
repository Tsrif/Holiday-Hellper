using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Interact : MonoBehaviour {

    public bool nearChild;
    public bool carrying;
   // public GameObject kid;
    public Transform holder;
    private Rigidbody rb;
    public Interactable currentInteractable;
    public static event Action startButtonMash;
   

    public List<Interactable> interactWith = new List<Interactable>();

    private void Start()
    {
        nearChild = false;
        carrying = false;
    }

    private void OnEnable()
    {
        ButtonMash.buttonMash += buttonMash;
    }

    private void OnDisable()
    {
        ButtonMash.buttonMash -= buttonMash;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact")) {

            if (nearChild == true )
            {
                //kid.GetComponent<NavMeshAgent>().isStopped = true;
                //kid.transform.SetParent(holder.transform);
                // kid.transform.position = holder.transform.position;

                //carrying = true;
                //nearChild = false;

                if (startButtonMash!= null) { startButtonMash(); }
            }
            else if (carrying == true) {
                BringBackChild();
                //kid.transform.SetParent(null);
                carrying = false;
                nearChild = true;
            }
            else if(currentInteractable !=null) 
            {
                currentInteractable.doThis();
            }
        }
    }

    //If you activate the mini game then run away when you capture the child you won't be able to put it down
    private void OnTriggerEnter(Collider collision)
    {
        if (carrying == false) {

            if (collision.gameObject.tag == "Kid")
            {
                //Debug.Log("near kid");
                nearChild = true;
                //Debug.Log("Nearchild  " + nearChild);
                /* PLEASE FOR THE LOVE OF GOD FIX THIS THIS IS NOT A GOOD WAY TO DO THIS LMAOOOOOOOOOOOO*/
                // kid = collision.gameObject.transform.parent.gameObject.transform.parent.gameObject;
            }

            if (collision.gameObject.GetComponent<Interactable>() != null)
            {
                //Debug.Log("Added");
                Interactable addInteractable = collision.gameObject.GetComponent<Interactable>();
                currentInteractable = addInteractable;
                //interactWith.Add(addInteractable);

            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Kid")
        {
            //Debug.Log("Nearchild  " + nearChild);
            nearChild = false;
        }

        if (collision.gameObject.GetComponent<Interactable>() == currentInteractable)
        {
            currentInteractable = null;
        }
    }

    void HideCHild() {
        carrying = true;
        nearChild = false;
        //turn off kid script
        currentInteractable.GetComponent<Kid>().enabled = false;
        currentInteractable.GetComponent<NavMeshAgent>().enabled = false;
        //turn off visual part 
        foreach (Transform child in currentInteractable.transform)
        {
            child.gameObject.SetActive(false);
        }
        //parent to player 
        currentInteractable.transform.SetParent(holder.transform);
        //set posistion 
        currentInteractable.transform.position = holder.transform.position;
    }

    void BringBackChild() {
        //Unparent to player 
        currentInteractable.transform.SetParent(null);
        //turn on kid script
        currentInteractable.GetComponent<Kid>().enabled = true;
        currentInteractable.GetComponent<NavMeshAgent>().enabled = true;
        //turn on visual part 
        foreach (Transform child in currentInteractable.transform)
        {
            child.gameObject.SetActive(true);
        }

        //change player's state
        gameObject.GetComponent<PlayerController>()._playerState = PlayerState.IDLE;
    }

    //Recieves notifcation saying if player completed mini game or not 
    void buttonMash(bool completed) {

        if (completed) {
            HideCHild();
        }
        //Not sure what happens when you fail it 
       
    }
}
