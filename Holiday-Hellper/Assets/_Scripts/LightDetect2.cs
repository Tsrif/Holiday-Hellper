using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[RequireComponent(typeof(SphereCollider))]
public class LightDetect2 : MonoBehaviour
{

    //get the distance from the center of the light source to the center of the player 
    // Determine how much of the player is in the light
    //return a percentage 
    private float distance;
    public float percentVisible;
    public GameObject player;
    private SphereCollider sc;

    public static event Action<float> PercentVisible;

    private void OnDisable()
    {
        percentVisible = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            //Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.yellow);
            percentVisible = ((1 / distance) * 10) * 2; //there's probably a better formula but this is what I came up with
            percentVisible = Mathf.Clamp(percentVisible, 0, 0.99f);
            sendNotif(percentVisible);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        percentVisible = 0;
        sendNotif(percentVisible);
    }

    //created a seperate method for sending notifcations so a notifcation can be sent whent he head light gets turned off
    public void sendNotif(float percent) {
        if (PercentVisible != null) { PercentVisible(percent); }
    }
}
