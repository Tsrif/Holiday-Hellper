using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HoleController : MonoBehaviour {

    public static event Action winCondition;

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
