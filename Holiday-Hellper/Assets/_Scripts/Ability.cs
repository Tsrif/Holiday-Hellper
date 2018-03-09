using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ability : MonoBehaviour
{

    public GameState gameState; //the state the game is in
    public PlayerState playerState; //the state the player is in
    public int manaCost = 1; //default mana cost value 
    public static event Action<String, int> ManaSend; //notification to send to manaBar 
    public bool okayToUse; //bool if ability can be used 

    public void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        PlayerController.CurrentState += updatePlayerState;
        ManaBar.useAbility += useAbility;
    }

    public void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        PlayerController.CurrentState -= updatePlayerState;
        ManaBar.useAbility -= useAbility;
    }

    //circumstances where an ability can NOT be used 
    protected bool CheckExceptions()
    {
        if (gameState == GameState.PAUSED || gameState == GameState.WIN || gameState == GameState.LOSE || playerState == PlayerState.CARRYING || okayToUse)
        {
            return false;
        }
        return true;
    }

    protected void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    protected void updatePlayerState(PlayerState state)
    {
        this.playerState = state;
    }

    //Send a notification to see if an ability is okay to use 
    protected void CheckOkay(String sendString, int cost)
    {
        if (ManaSend != null)
        {
            ManaSend(this.GetType().ToString(), manaCost);
        }
    }

    //returns if an ability is okay to use or not 
    protected void useAbility(String me)
    {
        if (me == this.GetType().ToString())
        {
            okayToUse = true;
        }
    }
}
