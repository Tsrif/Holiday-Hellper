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
        if (Input.GetButtonDown("Ghost"))
        {
            Debug.Log("Ghost");

            resetColor();
            myMenu.selected = myMenu.Ghost;
            myMenu.Ghost.circle.color = Color.white;


        }
        else if (Input.GetButtonDown("Hide"))
        {
            Debug.Log("Hide");

            resetColor();
            myMenu.selected = myMenu.Hide;
            myMenu.Hide.circle.color = Color.white;


        }
        else if (Input.GetButtonDown("Decoy"))
        {
            Debug.Log("Decoy");

            resetColor();
            myMenu.selected = myMenu.Decoy;
            myMenu.Decoy.circle.color = Color.white;


        }
        else if (Input.GetButtonDown("Stun"))
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
