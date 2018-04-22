using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HouseCompletionCounter : MonoBehaviour {
    private static int completed = 0;
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

    public static int completionNum()
    {
        return completed;
    }
}

