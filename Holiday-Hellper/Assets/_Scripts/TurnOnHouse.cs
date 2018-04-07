using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnHouse : MonoBehaviour {
    public int lastHouseId;
    private void Start()
    {
        HouseWin.turnNextHouseOn += turnOn;
        StartCoroutine(delay());
    }

    //Delay used kind of like a late start
    IEnumerator delay() {
        yield return new WaitForSeconds(0.5f);
        turnOff();
        StopAllCoroutines();
    }

    void turnOn(int id) {
        if (id == lastHouseId) {
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    void turnOff() {
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
