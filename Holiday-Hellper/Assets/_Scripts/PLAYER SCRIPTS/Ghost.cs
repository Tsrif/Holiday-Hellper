using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ghost : Ability {
    public GameObject player;
    public static event Action<bool> ghost; //send notification to turn into a ghost 
    public float ghostTime;
	// Update is called once per frame
	void Update () {
        if (!CheckExceptions()) { return; }

        if (Input.GetButtonDown("UseAbility"))
        {
            CheckOkay(this.GetType().ToString(), manaCost);
            //check to see if it's okay to use the ability
            if (okayToUse)
            {
                //if okay then ghost 
                if (ghost != null)
                {
                    player.GetComponent<Interact>().enabled = false;
                    StartCoroutine(GhostTimer(ghostTime));
                    ghost(true);
                }
            }
        }
    }

    public IEnumerator GhostTimer(float time) {
        okayToUse = false;
        yield return new WaitForSeconds(time);
        //if okay then ghost 
        if (ghost != null)
        {
            player.GetComponent<Interact>().enabled = true;
            ghost(false);
        }
    }
}
