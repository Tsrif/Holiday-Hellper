using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : Interactable
{
    public bool on = true;
    public Light[] arrayOfLights;

    public void Start()
    {
        _type = INTERACTABLETYPE.LIGHTSWITCH;
        //on = true;
        doThis = switchLightState;
        switchLightState();
    }

    public void switchLightState()
    {
        if (on == true)
        {
            for (int i = 0; i < arrayOfLights.Length; i++)
            {
                arrayOfLights[i].enabled = false;
                on = false;
            }
        }
        else
        {
            for (int i = 0; i < arrayOfLights.Length; i++)
            {
                arrayOfLights[i].enabled = true;
                on = true;
            }
        }
    }
}
