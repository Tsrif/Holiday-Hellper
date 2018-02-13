using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDetection : MonoBehaviour {
    bool keepRunning = true;
    PlayerController player;
    bool lightOn;
    RaycastHit[] obstacles;

    public void OnEnable()
    {
        lightOn = true;
    }
    public void OnDisable()
    {
        lightOn = false;
        if (player != null) {
            player._playerVisibility = PlayerVisibility.NOTVISIBLE;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            player = other.gameObject.GetComponent<PlayerController>();
            StartCoroutine("checkVisibility");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            player._playerVisibility = PlayerVisibility.NOTVISIBLE;
            //Debug.Log(player._playerVisibility);
            player = null;
            StopCoroutine("checkVisibility");
        }
    }

    IEnumerator checkVisibility() {
        while (keepRunning == true) {
            if (lightOn == true) {
                //Debug.Log("Light on");
                obstacles = Physics.RaycastAll(transform.position, player.transform.position - transform.position, Vector3.Distance(transform.position, player.transform.position));
                bool check = false;
                for (int i = 0; i < obstacles.Length; i++) {
                    if (obstacles[i].collider.tag == "wall") {
                        check = true;
                    }
                }
                if (check == false) {
                    player._playerVisibility = PlayerVisibility.VISIBLE;
                }
            }
            Debug.Log(player._playerVisibility);
            yield return null;
        }
    }

}
