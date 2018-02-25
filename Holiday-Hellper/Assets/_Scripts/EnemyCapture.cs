using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCapture : MonoBehaviour
{
    public GameObject target;
    public static event Action caught;
    public float time;
    public float countDown;
    [SpaceAttribute]
    public GameObject bar;
    public float increment;
    public bool inside;
    public Patrol patrol;


    private void Start()
    {
        bar.GetComponent<Slider>().maxValue = time;
        bar.SetActive(false);
    }

    private void OnEnable()
    {
        Hide.hide += playerHidden;
    }

    private void OnDisable()
    {
        Hide.hide -= playerHidden;
    }


    private void Update()
    {
        bar.GetComponent<Slider>().value = countDown;
        if (!inside && countDown > 0)
        {
            countDown -= increment * Time.deltaTime;
            countDown = Mathf.Clamp(countDown, 0, time);
        }
        if (countDown == 0) { bar.SetActive(false); }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == target && patrol._patrolState != PatrolState.STUNNED)
        {
            bar.SetActive(true);
            inside = true;
            if (countDown < time)
            {
                countDown += increment * Time.deltaTime;
                countDown = Mathf.Clamp(countDown, 0, time);
            }
            else if (countDown == time)
            {
                if (caught != null)
                {
                    caught();
                }
            }
        }
    }

    //inside set to false on exit
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            inside = false;
        }
    }

    //inside set to false when player is hidden 
    void playerHidden(HideState state)
    {
        if (state == HideState.HIDDEN) { inside = false; }
    }
}
