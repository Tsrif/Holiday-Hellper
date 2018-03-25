using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coward : Kid {
    public KidState behavior; 

	// Use this for initialization
	void Start () {
        hearingRadius = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        if (canHear) { _kidState = behavior; }
	}
}
