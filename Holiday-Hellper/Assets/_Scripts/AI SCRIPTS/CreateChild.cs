using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CreateChild : MonoBehaviour
{

    //Child Type
    public KidState kidType;
    //Child Spawn Locations 
    public List<Transform> spawnSpots = new List<Transform>();
    //patrol locations 
    public List<Transform> patrolPoints = new List<Transform>();
    //child prefab
    public GameObject childPrefab;

    // Use this for initialization
    void Start()
    {
        //Create Spawn Position
        Transform spawnPos = spawnSpots[UnityEngine.Random.Range(0, spawnSpots.Count)];
        Quaternion rotation = transform.rotation;
        //instantiate child prefab
        GameObject child = Instantiate(childPrefab, spawnPos.position, rotation);
        //Set type of child
        kidType = (KidState)UnityEngine.Random.Range(0, 5);
        child.GetComponent<Kid>().type = kidType;
        //Set patrol locations 
        child.GetComponent<Kid>().patrolPoints[0] = patrolPoints[UnityEngine.Random.Range(0, patrolPoints.Count)];
        child.GetComponent<Kid>().patrolPoints[1] = patrolPoints[UnityEngine.Random.Range(0, patrolPoints.Count)];
    }
}
