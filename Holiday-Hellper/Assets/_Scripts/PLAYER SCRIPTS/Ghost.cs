using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ghost : Ability {
    public static event Action<bool> ghost; //send notification to turn into a ghost 
    public float ghostTime;
	// Update is called once per frame
	void Update () {
        if (!CheckExceptions()) { return; }

        if (Input.GetKey(KeyCode.G))
        {
            CheckOkay(this.GetType().ToString(), manaCost);
            //check to see if it's okay to use the ability
            if (okayToUse)
            {
                //if okay then ghost 
                if (ghost != null)
                {
                    StartCoroutine(GhostTimer(5f));
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
            ghost(false);
        }
    }
}
