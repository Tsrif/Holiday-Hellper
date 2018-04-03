using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ABILITIES {GHOST, HIDE, DECOY, STUN };
public class AbilityManager : MonoBehaviour {
    public Hide hide;
    public CreateDecoy decoy;
    public Stun stun;
    public Ghost ghost;

    public ABILITIES currentAbility;
	// Use this for initialization
	void Start () {
        disableAll();
	}
    private void OnEnable()
    {
        RadialMenu.selectedAbility += changeSelected;
    }

    private void OnDisable()
    {
        RadialMenu.selectedAbility -= changeSelected;
    }

    // Update is called once per frame
    void Update () {
        changeState();	
	}

    void changeState() {
        switch (currentAbility) {
            case ABILITIES.GHOST:
                ghost.enabled = true;
                break;

            case ABILITIES.HIDE:
                hide.enabled = true;
                break;

            case ABILITIES.DECOY:
                decoy.enabled = true;
                break;

            case ABILITIES.STUN:
                stun.enabled = true;
                break;
        }
    }

    void changeSelected(int selected) {
        if (selected != (int)currentAbility) { disableAll(); }
        currentAbility = (ABILITIES)selected;
    }

    void disableAll() {
        hide.enabled = false;
        decoy.enabled = false;
        stun.enabled = false;
        ghost.enabled = false;
    }
}
