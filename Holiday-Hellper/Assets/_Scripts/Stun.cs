using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stun : MonoBehaviour
{
    public static event Action<float> stun;
    public static event Action<int> manaSend;
    private GameState gameState;
    public float stunTime; //how long the stun will last
    public float coolDown; //How long before you can use the ability again after using it
    public bool start; //true if the timer has been started
    public int manaCost;
    public bool okayToUse;

    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        ManaBar.useAbility_Stun += useAbility;

    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        ManaBar.useAbility_Stun -= useAbility;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.PAUSED || gameState == GameState.WIN)
        {
            return;
        }

        if (Input.GetButtonDown("Stun") && !start)
        {
            //check to see if it's okay to use the ability
            if (manaSend != null) { manaSend(manaCost); }
            //if not okay then return;
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

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void useAbility()
    {
        okayToUse = true;
    }
}
