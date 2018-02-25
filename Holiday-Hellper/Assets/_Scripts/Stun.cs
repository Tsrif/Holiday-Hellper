using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stun : MonoBehaviour {
    public static event Action<float> stun;
    private GameState gameState;
    public float stunTime; //how long the stun will last
    public float coolDown; //How long before you can use the ability again after using it
    public bool start; //true if the timer has been started

    void OnEnable()
    {
        GameController.changeGameState += updateGameState;

    }

    void OnDisable()
    {
        GameController.changeGameState -= updateGameState;

    }

	// Update is called once per frame
	void Update () {
        if (gameState == GameState.PAUSED || gameState == GameState.WIN)
        {
            return;
        }

        if (Input.GetButtonDown("Stun") && !start)
        {
            if (stun != null)
            {
                StartCoroutine(CoolDown(coolDown));
                stun(stunTime);
            }
        }
	}

    IEnumerator CoolDown(float time) {
        start = true;
        yield return new WaitForSeconds(time);
        start = false;
        StopCoroutine(CoolDown(time));
    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

}
