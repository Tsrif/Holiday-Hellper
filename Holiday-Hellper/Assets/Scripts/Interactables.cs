using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{

    public string name;
    public delegate void myDelegate();
    public myDelegate doThis;

    public void setName(string newName)
    {
        name = newName;
    }
    public string getName()
    {
        return name;
    }
}