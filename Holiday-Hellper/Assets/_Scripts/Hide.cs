﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum HideState { HIDDEN, NOT_HIDDEN };

public class Hide : MonoBehaviour
{

    public GameObject player;
    public GameObject playerVisual;
    public GameObject hole;
    public HideState _hideState;
    public Transform feetSpot;
    public float groundHeight;

    public int hideCount;
    public int hideLimit;
    public static event Action<HideState> hide; //Hide sends notification to player to change states, player then broadcasts what state they are in 
    public Vector3 lastPos;

    private GameState gameState;
    public int manaCost;
    public static event Action<int> manaSend;
    public bool okayToUse;

    void Start()
    {
        _hideState = HideState.NOT_HIDDEN;
    }
    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        //get from manaBar
        ManaBar.useAbility_Hide += useAbility;
    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        ManaBar.useAbility_Hide -= useAbility;
    }

    //When the hide button is pressed, then hide. 
    //Uses States to keep track if hidden or not
    //Hiding works by just enabling and disabling certain components,
    //so the player is no longer visible. 
    void Update()
    {
        if (gameState == GameState.PAUSED || gameState == GameState.WIN)
        {
            return;
        }

        if (Input.GetButtonDown("Hide"))
        {
            if (_hideState == HideState.NOT_HIDDEN)
            {
                //check to see if it's okay to use the ability
                if (manaSend != null) { manaSend(manaCost); }
                //If we hit the limit or are carrying something, then don't hide anymore. 
                if (hideCount == hideLimit || player.GetComponent<PlayerController>()._playerState == PlayerState.CARRYING || !okayToUse)
                {
                    return;
                }
                _hideState = HideState.HIDDEN;

                switchState();

            }
            else
            {
                _hideState = HideState.NOT_HIDDEN;
                switchState();
            }
        }

    }


    void switchState()
    {
        switch (_hideState)
        {
            case HideState.HIDDEN:
                hideStuff();
                hideCount++;
                okayToUse = false;
                break;
            case HideState.NOT_HIDDEN:
                unHide();
                hole.GetComponent<MeshRenderer>().enabled = false;
                //hole.SetActive(false);
                break;
            default:
                break;
        }
        if (hide != null)
        {
            hide(_hideState);
        }
    }
    private void OnGUI()
    {
        Rect rect = new Rect(100, 10, 100, 20);
        GUI.Label(rect, "Hides Left: " + (hideLimit - hideCount));
    }

    void hideStuff()
    {
        player.GetComponent<CharacterController>().enabled = false;
        // player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<Interact>().enabled = false;

        playerVisual.SetActive(false);

        hole.GetComponent<MeshRenderer>().enabled = true;
        hole.transform.position = new Vector3(feetSpot.position.x, feetSpot.position.y + groundHeight, feetSpot.position.z);
    }

    void unHide()
    {
        player.GetComponent<CharacterController>().enabled = true;
        // player.GetComponent<PlayerController>().enabled = true;
        player.GetComponent<Interact>().enabled = true;
        hole.GetComponent<MeshRenderer>().enabled = false;
        playerVisual.SetActive(true);
    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void useAbility()
    {
        okayToUse = true;
    }
}
