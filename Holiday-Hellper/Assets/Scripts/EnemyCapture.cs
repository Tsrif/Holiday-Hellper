using System;
using UnityEngine;

public class EnemyCapture : MonoBehaviour {

    public GameObject target;
    public static event Action caught;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            

            if (caught != null) {
                Debug.Log("Caught has value");
                caught();
            }
        }
    }
}
