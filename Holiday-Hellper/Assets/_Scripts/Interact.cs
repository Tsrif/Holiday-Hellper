using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public bool nearChild;
    public bool carrying;
    private GameObject kid;
    public Transform holder;
    private Rigidbody rb;

    public List<Interactable> interactWith = new List<Interactable>();

    private void Start()
    {
        nearChild = false;
        carrying = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Interact")) {

            if (nearChild == true)
            {

                kid.transform.SetParent(holder.transform);
                rb = kid.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                kid.transform.position = holder.transform.position;

                carrying = true;
                nearChild = false;
            }
            else if (carrying == true) {
                kid.transform.SetParent(null);
                rb = kid.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                carrying = false;
                nearChild = true;
            }
            else if(interactWith.Count > 0) 
            {
                interactWith[0].doThis();
            }
        }
    }

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
            interactWith.Add(addInteractable);

        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Kid")
        {
            //Debug.Log("Nearchild  " + nearChild);
            nearChild = false;
        }

        if (collision.gameObject.GetComponent<Interactable>() != null)
        {
            Interactable removeInteractable = collision.gameObject.GetComponent<Interactable>();
            interactWith.Remove(removeInteractable);
        }

    }
}
