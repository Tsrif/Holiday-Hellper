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
    public GameState gameState;


    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        bar.GetComponent<Slider>().maxValue = time;
        bar.SetActive(false);
    }

    private void OnEnable()
    {
        GameController.changeGameState += updateGameState;
        Hide.hide += playerHidden;
    }

    private void OnDisable()
    {
        GameController.changeGameState -= updateGameState;
        Hide.hide -= playerHidden;
    }


    private void Update()
    {
        if (gameState == GameState.PAUSED || gameState == GameState.WIN || gameState == GameState.LOSE)
        {
            return;
        }

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
        if (gameState == GameState.PAUSED || gameState == GameState.WIN || gameState == GameState.LOSE)
        {
            return;
        }
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

    void updateGameState(GameState gameState)
    {
        this.gameState = gameState;
    }

    //inside set to false when player is hidden 
    void playerHidden(HideState state)
    {
        if (state == HideState.HIDDEN) { inside = false; }
    }
}
