using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    public List<string> text = new List<string>();
    public List<string> input = new List<string>();
    public Text panelText;
    public GameObject start;

    public int checkText;
    public int checkInput;

    public GameObject door;

    private void Start()
    {
       panelText.text = text[checkText];
    }

    // Update is called once per frame
    void Update () {
        panelText.text = text[checkText];
        if (Input.GetButtonDown(input[checkInput])) {
            checkInput += 1;
            checkText += 1;
        }
        if (checkInput == 3)
        {
            door.SetActive(true);
        }

        if(checkInput == input.Count-1)
        {
            start.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
           
        }

               
	}
}
