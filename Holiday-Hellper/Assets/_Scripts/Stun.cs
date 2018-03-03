using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stun : MonoBehaviour
{
    public static event Action<float> stun; //notifcation to send to patrol to stun them 
    public float stunTime; //how long the stun will last
    public float coolDown; //How long before you can use the ability again after using it
    public bool start; //true if the timer has been started

    private GameState gameState;
    public int manaCost;
    public static event Action<String, int> manaSend; //notification to send to manaBar 
    public bool okayToUse;

    void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        ManaBar.useAbility += useAbility;

    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        ManaBar.useAbility -= useAbility;
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
			if (manaSend != null) { manaSend(this.GetType().ToString(),manaCost); }
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

	void useAbility(String me)
	{
		if(me == this.GetType().ToString()){
			okayToUse = true;
		}

	}
}
