using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spotted : MonoBehaviour {

    private Text text;
    public bool spotted;

    private void Start()
    {
        text = GetComponent<Text>();
        text.text = "Hidden";
    }
    private void OnEnable()
    {
        Patrol.spottedPlayer += playerSpotted;
    }

    private void OnDisable()
    {
        Patrol.spottedPlayer -= playerSpotted;
    }

    void playerSpotted(bool seen) {
        if (seen) { text.text = "Spotted!"; }
        else if(!seen) { text.text = "Hidden"; }
    }
}
