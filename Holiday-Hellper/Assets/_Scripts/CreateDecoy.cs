using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Create a decoy object in front of the player 
public class CreateDecoy : Ability
{
    public GameObject decoyPrefab;
    public GameObject player;
    public static event Action<GameObject> decoySend;
    [SpaceAttribute]
    public float moveSpeed;
    public float time;
    public float spawnDistance;
    public float gravity;
    public GameObject SmokeEffect;

    // Update is called once per frame
    void Update()
    {
        if (!CheckExceptions()) { return; }
        if (Input.GetButtonDown("Decoy"))
        {
            //check to see if it's okay to use the ability
            CheckOkay(this.GetType().ToString(), manaCost);
            if (okayToUse)
            {
                createDecoy();
                okayToUse = false;
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
        ds.SmokeEffect = SmokeEffect;
    }
}
