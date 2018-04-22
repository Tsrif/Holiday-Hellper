using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadHouses : MonoBehaviour {

    private int timer;
    public GameObject[] houses;

    private void Start()
    {
        //timer = 20;
        //StartCoroutine("loading");
    }

    IEnumerator loading() {
        for (int i = 0; i < houses.Length; i++) {
            //yield return new WaitForSeconds(timer);
            yield return new WaitForSeconds(10);
            Instantiate(houses[i]);
            //timer -= 5;
        }
    }
    private void OnLevelWasLoaded(int level)
    {
        StartCoroutine("loading");
    }
}
