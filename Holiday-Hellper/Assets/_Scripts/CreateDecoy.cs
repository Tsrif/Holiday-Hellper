using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//Currently the decoy just spawns in front of the player then moves forward for an amount of time
//need to add in the functionality that the patrol changes its target to the decoy

//Create a decoy object in front of the player 
public class CreateDecoy : MonoBehaviour
{
    public GameObject decoyPrefab;
    public GameObject player;
    private GameState gameState;
    public int manaCost;
    public static event Action<String, int> manaSend; //notification to send to manaBar 
    public static event Action<GameObject> decoySend;
    public bool okayToUse;
    [SpaceAttribute]
    public float moveSpeed;
    public float time;
    public float spawnDistance;
    public float gravity;

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (manaSend != null) { manaSend(this.GetType().ToString(), manaCost); }
            if (okayToUse)
            {
                okayToUse = false;
                createDecoy();
                
            }
        }
    }

    void createDecoy()
    {
        Vector3 playerPos = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Quaternion playerRotation = player.transform.rotation;
        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
        //instantiate decoy prefab
        GameObject decoy = Instantiate(decoyPrefab, spawnPos, playerRotation);
        // add decoy script to it with custom info I guess 
        DecoyFactory(decoy, moveSpeed, player.transform.forward, time, gravity);
        if (decoySend != null) { decoySend(decoy); } //send notification to patrol with the decoy gameObject
    }

    //add decoy script to to decoy object then set info. 
    public void DecoyFactory(GameObject decoy, float moveSpeed, Vector3 moveDir, float time, float gravity)
    {
        var ds = decoy.GetComponent<Decoy>();
        ds._moveSpeed = moveSpeed;
        ds._aliveTime = time;
        ds._gravityScale = gravity;
        ds._moveDirection = moveDir;
    }

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    void useAbility(String me)
    {
        if (me == this.GetType().ToString())
        {
            okayToUse = true;
        }
    }
}
