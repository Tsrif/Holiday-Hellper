using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolSpawner : MonoBehaviour {
    public List<Transform> patrolPoints = new List<Transform>();
    public GameObject patrolPrefab;
    public GameObject parentPrefab;
    public GameObject dogPrefab;
    [SpaceAttribute]
    public int patrolAmount;
    public int parentAmount;
    public int dogAmount;
    public float distanceAmount;

    void Start () {
        createPatrol();
        createParent();
        createDog();
	}
	

    void createPatrol() {
        for (int i = 0; i < patrolAmount; i++) {
            Transform spawnPos = patrolPoints[Random.Range(0, patrolPoints.Count)];
            //print(spawnPos);
            Quaternion rotation = transform.rotation;
            //instantiate patrol prefab
            GameObject patrol = Instantiate(patrolPrefab, spawnPos.position, rotation);
            patrol.GetComponent<Patrol>().patrolPoints[0] = patrolPoints[Random.Range(0, patrolPoints.Count)];
            patrol.GetComponent<Patrol>().patrolPoints[1] = patrolPoints[Random.Range(0, patrolPoints.Count)];
        }
    }

    void createParent() {
        for (int i = 0; i < parentAmount; i++)
        {
            Transform spawnPos = patrolPoints[Random.Range(0, patrolPoints.Count)];
            Quaternion rotation = transform.rotation;
            //instantiate decoy prefab
            GameObject parent = Instantiate(parentPrefab, spawnPos.position, rotation);
            parent.GetComponent<Patrol>().patrolPoints[0] = patrolPoints[Random.Range(0, patrolPoints.Count)];
            parent.GetComponent<Patrol>().patrolPoints[1] = patrolPoints[Random.Range(0, patrolPoints.Count)];
        }
    }

    void createDog() {
        for (int i = 0; i < dogAmount; i++)
        {
            Transform spawnPos = patrolPoints[Random.Range(0, patrolPoints.Count)];
            Quaternion rotation = transform.rotation;
            //instantiate decoy prefab
            GameObject dog = Instantiate(dogPrefab, spawnPos.position, rotation);
            dog.GetComponent<Patrol>().patrolPoints[0] = patrolPoints[Random.Range(0, patrolPoints.Count)];
            dog.GetComponent<Patrol>().patrolPoints[1] = patrolPoints[Random.Range(0, patrolPoints.Count)];
        }
    }
}
