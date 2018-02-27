using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{

    //recieve notification from abilities about how much mana to subtract
    //if that amount of mana is available send notification back to sender to let them know if they can or not 
    //subtract that amount of mana
    //every x amount of seconds slowly increase mana to max amount 

    public int manaPool;
    public int max;

    public float time;
    [SpaceAttribute]
    public GameObject bar;
    public float increment;
    public static event Action useAbility_Hide;
    public static event Action useAbility_Stun;
    public float fillAmount;
    public bool refill;

    // Use this for initialization
    void Start()
    {
        manaPool = max;
        bar.GetComponent<Image>().fillAmount = manaPool;
        fillAmount = bar.GetComponent<Image>().fillAmount;
    }

    private void OnEnable()
    {
        Hide.manaSend += hide;
        Stun.manaSend += stun;

    }

    private void OnDisable()
    {
        Hide.manaSend -= hide;
        Stun.manaSend -= stun;
    }

    // Update is called once per frame
    void Update()
    {
        bar.GetComponent<Image>().fillAmount = fillAmount;
        fillAmount = Mathf.Clamp(fillAmount, 0, max);
        if (manaPool == max)
        {
            refill = false;
            //manaPool = Mathf.Clamp(manaPool, 0, max);
        }
        if (refill)
        {
            fillAmount += increment * Time.deltaTime;
            fillAmount = Mathf.Clamp(fillAmount, 0, 1);
            manaPool = (int)(fillAmount * 10);
        }
    }

    IEnumerator RefillBar(float time)
    {

        yield return new WaitForSeconds(time);
        if (manaPool <= max) { refill = true; }

    }


    //recieve notification from ability, if we can afford to use it send notifcation to ability that it's okay
    void hide(int cost)
    {
        if (manaPool - cost >= 0)
        {
            manaPool -= cost;
            fillAmount -= 0.1f * cost;
            if (useAbility_Hide != null) { useAbility_Hide(); }
            StartCoroutine(RefillBar(time));
        }
    }


    void stun(int cost)
    {
        if (manaPool - cost >= 0)
        {
            manaPool -= cost;
            fillAmount -= 0.1f * cost;
            if (useAbility_Stun != null) { useAbility_Stun(); }
            StartCoroutine(RefillBar(time));
        }
    }
}
