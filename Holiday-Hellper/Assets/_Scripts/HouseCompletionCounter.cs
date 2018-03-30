using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HouseCompletionCounter : MonoBehaviour {
    private int completed;
    public int max;
    public static event Action win;
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
        if (completed == max) {
            if (win!=null) { win(); }
        }
    }
}

