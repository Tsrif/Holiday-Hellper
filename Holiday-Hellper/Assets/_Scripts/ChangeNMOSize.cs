using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle))]

public class ChangeNMOSize : MonoBehaviour {
    private NavMeshObstacle obstacle;
    public float newSize;
    // Use this for initialization
    void Start () {
        obstacle = GetComponent<NavMeshObstacle>();
        if (transform.localScale.x > transform.localScale.z)
        {
            obstacle.size = new Vector3(obstacle.size.x, obstacle.size.y, newSize);
        }
        else if (transform.localScale.z > transform.localScale.x)
        {
            obstacle.size = new Vector3(newSize, obstacle.size.y, obstacle.size.z);
        }
    }
	
}
