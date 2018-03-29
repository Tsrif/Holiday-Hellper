using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseCompletionCounter : MonoBehaviour {
    private int completed;
    public int max;
    private void OnEnable()
    {
        HouseWin.houseCompleted += houseIncrement;
    }

    private void OnDisable()
    {
        HouseWin.houseCompleted -= houseIncrement;
    }

    void houseIncrement() {
        completed++;
        gameObject.GetComponent<Text>().text = "Completed: " + completed;
    }
}

