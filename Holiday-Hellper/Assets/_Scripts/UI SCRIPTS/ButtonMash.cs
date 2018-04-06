using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ButtonMash : MonoBehaviour {

    public float meter;
    public float fullMeter;
    public float mashIncrement;
    public float mashDecrement;
    public bool start;
    public float minStart;
    public GameObject fullBar;

    public GameObject bar;
    //Notifcation that tells if mini game was completed or not 
    public static event Action<bool> buttonMash;
    //Send notification when player has started mini game
    public static event Action<bool> captureStart; //true if start, false if ended 
    private void Start()
    {
        bar.GetComponent<Image>().fillAmount = 0;
        fullBar.SetActive(false);
    }
    private void OnEnable()
    {
        Interact.startButtonMash += recieveNotif;
    }

    private void OnDisable()
    {
        Interact.startButtonMash -= recieveNotif;
    }

    private void Update()
    {
        if (start) {
            StartCoroutine(SubtractMeter());
            ButtonMasher();
            if (meter == 0) {
                sendToInteract(false);
                if (captureStart != null) { captureStart(false); }
            }

            if (meter == fullMeter) {
                sendToInteract(true);
                if (captureStart != null) { captureStart(false); }
            }
        }
    }

    void sendToInteract(bool completed) {
        StopAllCoroutines();
        start = false;
        if (buttonMash != null)
        {
            buttonMash(completed);
        }
        fullBar.SetActive(false);
    }

    void ButtonMasher()
    {
        if (Input.GetButtonDown("Mash"))
        {
            meter += mashIncrement;
        }
       
        bar.GetComponent<Image>().fillAmount = meter/100;
        meter = Mathf.Clamp(meter, 0, fullMeter);
    }

    IEnumerator SubtractMeter() {
        yield return new WaitForSeconds(2f);
        meter -= Time.deltaTime * mashDecrement;
    }

    void recieveNotif() {
        if (captureStart != null) { captureStart(true); }
        fullBar.SetActive(true);
        start = true;
        meter = minStart;
    }
}
