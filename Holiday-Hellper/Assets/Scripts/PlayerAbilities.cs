using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAbilities : MonoBehaviour {

    public GameObject limbo;
    public GameObject myPos;
    public GameObject hideSpot;
    public Interact interact;

    public Vector3 posHolder;
    public Transform feetSpot;

    public bool isHidden;
    public int hideCount;
    public int hideLimit;
    public static event Action hide;
    public float height;


	// Use this for initialization
	void Start () {

        interact = GetComponent<Interact>();
        hideSpot.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetButtonDown("Hide"))
        {
            if(isHidden)
            {
                if (hide != null)
                {
                    hide();
                }
                hideSpot.SetActive(false);
                myPos.transform.position = posHolder;
                isHidden = false;
            } else if (!isHidden && !interact.carrying && hideCount < hideLimit)
            {
                if (hide != null) {
                    hide();
                }              
                posHolder = feetSpot.position;
                myPos.transform.position = limbo.transform.position;
                hideSpot.transform.position = new Vector3(posHolder.x,posHolder.y + height,posHolder.z);
                hideSpot.SetActive(true);
                isHidden = true;
                hideCount++;
            }
            

        }
    }

    private void OnGUI()
    {
        Rect rect = new Rect(100, 10, 100,20);
        GUI.Label(rect, "Hides Left: " + (hideLimit - hideCount));
    }
}
