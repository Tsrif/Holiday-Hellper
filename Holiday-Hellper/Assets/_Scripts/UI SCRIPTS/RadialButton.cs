using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;


public class RadialButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Image circle;
    public Image icon;
    public string title;
    public RadialMenu myMenu;
    public int pos;

    Color defaultColor;
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
}

