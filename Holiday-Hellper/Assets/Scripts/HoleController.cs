using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HoleController : MonoBehaviour {

    public static event Action winCondition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Kid")
        {
            if(winCondition != null)
            {
                winCondition();
            }
        }
    }
}
