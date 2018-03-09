using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stun : Ability
{
    public static event Action<float> stun; //notifcation to send to patrol to stun them 
    public float stunTime; //how long the stun will last
    public float coolDown; //How long before you can use the ability again after using it
    public bool start; //true if the timer has been started

    // Update is called once per frame
    void Update()
    {
        if (!CheckExceptions()) { return; }

        if (Input.GetButtonDown("Stun") && !start)
        {
            CheckOkay(this.GetType().ToString(), manaCost);
            //check to see if it's okay to use the ability
            if (okayToUse)
            {
                //if okay then stun 
                if (stun != null)
                {
                    StartCoroutine(CoolDown(coolDown));
                    stun(stunTime);
                }
            }
        }
    }

    IEnumerator CoolDown(float time)
    {
        start = true;
        okayToUse = false;
        yield return new WaitForSeconds(time);
        start = false;
        StopCoroutine(CoolDown(time));
    }
}
