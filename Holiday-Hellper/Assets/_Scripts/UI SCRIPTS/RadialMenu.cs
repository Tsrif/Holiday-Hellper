using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RadialMenu : MonoBehaviour {


    public RadialButton button;
    public RadialButton selected;

    public static event Action<int> selectedAbility; //send notification to the ability manager

    // Use this for initialization
    public void SpawnButtons (InteractUI obj) {

        for(int i = 0; i<obj.options.Length; i++)
        {
            RadialButton newButton = Instantiate(button) as RadialButton;
            newButton.transform.SetParent(transform, false);

            //Spawn as a circle
            float theta = (2 * Mathf.PI / obj.options.Length) * i;
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);          
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 100f;

            newButton.circle.color = obj.options[i].color;
            newButton.icon.sprite = obj.options[i].sprite;
            newButton.title = obj.options[i].title;
            newButton.pos = i;
            newButton.myMenu = this;
        }
        
	}

    private void Update()
    {
        if (selectedAbility != null) {
            selectedAbility(selected.pos);
        }
    }
    //void Update()
    //{
    //    if(Input.GetButtonUp("SkillSelect"))
    //    {
    //        if(selected)
    //        {
    //            Debug.Log(selected.title + "was selected");
    //        }

    //        gameObject.SetActive(false);
    //        //Destroy(gameObject);
    //    }

    //}
}
