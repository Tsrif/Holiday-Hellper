using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/* Basic Idea:
 * When child gets into trigger
 * Remove all patrol and child from house
 * Show some kind of visual that informs player they have completed that house
 * Send a notification that adds +1 to win counter or something like that 
 */

public class HouseWin : MonoBehaviour {
    public GameObject kid;
    public static event Action houseCompleted;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kid")
        {
            //This is a gross way to get the child, but it works for now 
            kid = other.transform.parent.transform.parent.gameObject;
            //Clear the house
            clearHouse();
            //delete the enter trigger
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
            //send notification
            if (houseCompleted != null) { houseCompleted(); }
        }
    }

    void clearHouse() {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(false);
        }
        kid.SetActive(false);
    }
}
