using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Interact : MonoBehaviour {

    public bool nearChild;
    public bool carrying;
    private GameObject kid;
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

            if (nearChild == true && kid.GetComponent<Kid>()._kidState == KidState.STUNNED)
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
        if (collision.gameObject.tag == "Kid")
        {
            //Debug.Log("near kid");
            nearChild = true;
            //Debug.Log("Nearchild  " + nearChild);
            kid = collision.gameObject;
        }

        if(collision.gameObject.GetComponent<Interactable>() != null)
        {
            //Debug.Log("Added");
            Interactable addInteractable = collision.gameObject.GetComponent<Interactable>();
            currentInteractable = addInteractable;
            //interactWith.Add(addInteractable);

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
        kid.GetComponent<Kid>().enabled = false;
        kid.GetComponent<NavMeshAgent>().enabled = false;
        //turn off visual part 
        foreach (Transform child in kid.transform)
        {
            child.gameObject.SetActive(false);
        }
        //parent to player 
        kid.transform.SetParent(holder.transform);
        //set posistion 
        kid.transform.position = holder.transform.position;
    }

    void BringBackChild() {
        //Unparent to player 
        kid.transform.SetParent(null);
        //turn on kid script
        kid.GetComponent<Kid>().enabled = true;
        kid.GetComponent<NavMeshAgent>().enabled = true;
        //turn on visual part 
        foreach (Transform child in kid.transform)
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
