using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLight : MonoBehaviour {
    public GameObject lightSource;
    // Update is called once per frame
    public bool on;

    private void Start()
    {
        on = true;
    }
    void Update () {
        if (Input.GetButtonDown("Headlight")) {
            on = !on;
            change(on);
        }
        
	}

    void change(bool on) {
        if (on)
        {
            GetComponent<ParticleSystem>().Play();
            lightSource.SetActive(true);
        }
        if (!on)
        {
            GetComponent<ParticleSystem>().Stop();
            lightSource.GetComponent<LightDetect2>().sendNotif(0);
            lightSource.SetActive(false);
        }
    }
}
