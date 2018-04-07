using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Interact : MonoBehaviour
{
    public bool nearChild;
    public bool carrying;
    public Interactable kid;
    public Transform holder;
    private Rigidbody rb;
    public Interactable currentInteractable;
    public static event Action startButtonMash;


    //public List<Interactable> interactWith = new List<Interactable>();

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
        if (Input.GetButtonDown("Interact"))
        {
            if (nearChild == true)
            {
                //starts up the button mash mini game 
                if (startButtonMash != null) { startButtonMash(); }
            }
            //If we press the interact button and are carrying the child we will drop it 
            else if (carrying == true)
            {
                BringBackChild();
                //kid.transform.SetParent(null);
                carrying = false;
                nearChild = true;
            }
            //if we aren't carrying or near a child, currentInteractable 
            //isn't null and our current interactable isn't a child
            //then we interact with something else 
            else if (currentInteractable != null && currentInteractable._type != INTERACTABLETYPE.KID)
            {
                currentInteractable.doThis();
            }
        }
    }

    //If you activate the mini game then run away when you capture the child you won't be able to put it down
    private void OnTriggerEnter(Collider collision)
    {
        if (carrying == false)
        {
            //adds whatever interactable you're colliding with as your currentInteractable
            if (collision.gameObject.GetComponent<Interactable>() != null)
            {
                //Debug.Log("Added");
                Interactable addInteractable = collision.gameObject.GetComponent<Interactable>();
                currentInteractable = addInteractable;
                Debug.Log(currentInteractable._type);
                //interactWith.Add(addInteractable);
                if (currentInteractable._type == INTERACTABLETYPE.KID)
                {
                    kid = currentInteractable;
                    nearChild = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Kid")
        {
            nearChild = false;
            kid = null;
        }

        if (collision.gameObject.GetComponent<Interactable>() == currentInteractable)
        {
            currentInteractable = null;
        }
    }

    void HideCHild()
    {
        if (kid != null)
        {
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
    }

    void BringBackChild()
    {
        if (kid != null)
        {
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
    }

    //Recieves notifcation saying if player completed mini game or not 
    void buttonMash(bool completed)
    {

        if (completed)
        {
            HideCHild();
        }
        //Not sure what happens when you fail it 

    }
}
