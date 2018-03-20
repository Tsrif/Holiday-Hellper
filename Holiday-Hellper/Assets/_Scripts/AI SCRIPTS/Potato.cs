using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Potato child AI does nothing. Literally just lays there*/
public class Potato : Kid {

	// Use this for initialization
	void Start () {
        _kidState = KidState.SLEEPING;
	}
}
