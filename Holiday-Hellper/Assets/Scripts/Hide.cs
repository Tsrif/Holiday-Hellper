using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum HideState { HIDDEN, NOT_HIDDEN };
public class Hide : MonoBehaviour {

    public GameObject player;
    public GameObject playerVisual;
    public GameObject hole;
    public HideState _hideState;
    public Transform feetSpot;
    public float groundHeight;

    public int hideCount;
    public int hideLimit;
    public static event Action hide;

    void Start () {
        _hideState = HideState.NOT_HIDDEN;
	}
	
    //When the hide button is pressed, then hide. 
    //Uses States to keep track if hidden or not
    //Hiding works by just enabling and disabling certain components,
        //so the player is no longer visible. 
	void Update () {
        if (Input.GetButtonDown("Hide")) {
            if (_hideState == HideState.NOT_HIDDEN)
            {
                //If we hit the limit , then don't hide anymore. 
                if (hideCount == hideLimit) {
                    return;
                }
                _hideState = HideState.HIDDEN;
                switchState();
            }
            else {
                _hideState = HideState.NOT_HIDDEN;
                switchState();
            }
        }

    }


    void switchState() {
        if (hide != null)
        {
            hide();
        }
        switch (_hideState) {
            case HideState.HIDDEN:
                hideStuff();
                hideCount++;
                break;
            case HideState.NOT_HIDDEN:
                unHide();
                hole.GetComponent<MeshRenderer>().enabled = false;
                //hole.SetActive(false);
                break;
            default:
                break;
        }
    }
    private void OnGUI()
    {
        Rect rect = new Rect(100, 10, 100, 20);
        GUI.Label(rect, "Hides Left: " + (hideLimit - hideCount));
    }

    void hideStuff() {
        player.GetComponent<CharacterController>().enabled = false;
        player.GetComponent<PlayerController>().enabled = false;

        playerVisual.SetActive(false);

        hole.GetComponent<MeshRenderer>().enabled = true;
        hole.transform.position = new Vector3(feetSpot.position.x, feetSpot.position.y + groundHeight, feetSpot.position.z);
    }

    void unHide() {
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerController>().enabled = true;
        hole.GetComponent<MeshRenderer>().enabled = false;
        playerVisual.SetActive(true);
    }
}
