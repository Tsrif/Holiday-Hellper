using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;


public class RadialButton : MonoBehaviour
{

    public Image circle;
    public Image icon;
    public string title;
    public RadialMenu myMenu;
    public int pos;

    private void Start()
    {
        myMenu.selected = myMenu.Hide;
        myMenu.Hide.circle.color = Color.white;
    }
    void Update()
    {
        //xAxisD == 1 : Right
        //xAxisD == -1 : Left
        //yAxisD == 1 : Up
        //yAxisD == -1 : down
        if (Input.GetButtonDown("Ghost") || Input.GetAxis("yAxisD") == 1)
        {
            Debug.Log("Ghost");

            resetColor();
            myMenu.selected = myMenu.Ghost;
            myMenu.Ghost.circle.color = Color.white;


        }
        else if (Input.GetButtonDown("Hide") || Input.GetAxis("xAxisD") == 1)
        {
            Debug.Log("Hide");

            resetColor();
            myMenu.selected = myMenu.Hide;
            myMenu.Hide.circle.color = Color.white;


        }
        else if (Input.GetButtonDown("Decoy") || Input.GetAxis("yAxisD") == -1)
        {
            Debug.Log("Decoy");

            resetColor();
            myMenu.selected = myMenu.Decoy;
            myMenu.Decoy.circle.color = Color.white;


        }
        else if (Input.GetButtonDown("Stun") || Input.GetAxis("xAxisD") == -1)
        {
            Debug.Log("Stun");

            resetColor();
            myMenu.selected = myMenu.Stun;
            myMenu.Stun.circle.color = Color.white;


        }
    }

    public void resetColor()
    {
        myMenu.Ghost.circle.color = Color.red;
        myMenu.Hide.circle.color = Color.red;
        myMenu.Decoy.circle.color = Color.red;
        myMenu.Stun.circle.color = Color.red;

    }
    /*

public void OnPointerEnter(PointerEventData eventData)
    {
        myMenu.selected = this;
        defaultColor = circle.color;
        circle.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //myMenu.selected = null;
        circle.color = defaultColor;
    }
    */
}
