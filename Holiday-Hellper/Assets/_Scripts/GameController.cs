using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum GameState { PLAYING, PLAYING_TO_PAUSED, PAUSED, PAUSED_TO_PLAYING, WIN, LOSE };


public class GameController : MonoBehaviour
{

    public GameState _gameState;
    public GameObject PauseHolder;
    public GameObject Winholder;
    public GameObject loseHolder;
    //public GameObject defaultPause;
    //public GameObject defaultWin;
    //public EventSystem ES;
    public Text gameStateDisplay;

    public Button defaultWinButton;
    public Button defaultPauseButton;
    public Button defaultLoseButton;


    public static event Action<GameState> changeGameState;
    public GameState gameState
    {
        set
        {
            _gameState = value;
            if (changeGameState != null)
            {
                changeGameState(gameState);
            }
        }
        get
        {
            return _gameState;
        }
    }
    // Use this for initialization
    void Start()
    {
        //winUI.SetActive(false);
        PauseHolder.SetActive(false);
        Winholder.SetActive(false);
    }

    private void OnEnable()
    {
        HoleController.winCondition += win;
        EnemyCapture.caught += Lose;
    }
    private void OnDisable()
    {
        HoleController.winCondition -= win;
        EnemyCapture.caught -= Lose;
    }
    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.PLAYING:
                if (Input.GetButtonDown("Pause"))
                {
                    gameState = GameState.PLAYING_TO_PAUSED;
                }
                break;
            case GameState.PLAYING_TO_PAUSED:
                PauseHolder.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                switchToPaused();
                break;
            case GameState.PAUSED:
                if (Input.GetButtonDown("Pause"))
                {
                    gameState = GameState.PAUSED_TO_PLAYING;
                }
                break;
            case GameState.PAUSED_TO_PLAYING:
                PauseHolder.SetActive(false);
                defaultPauseButton.Select();
                Cursor.lockState = CursorLockMode.Locked;
                switchToPlaying();
                break;
        }
    }

    public void Continue()
    {
        gameState = GameState.PAUSED_TO_PLAYING;
    }

    public void switchToPlaying()
    {
        gameState = GameState.PLAYING;
        gameStateDisplay.text = "Playing";
    }

    public void switchToPaused()
    {
        gameState = GameState.PAUSED;
        gameStateDisplay.text = "Paused";
    }

    public void caught()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void win()
    {
        Cursor.lockState = CursorLockMode.None;
        gameState = GameState.WIN;
        defaultWinButton.Select();
        Winholder.SetActive(true);
    }

    public void Lose()
    {
        Cursor.lockState = CursorLockMode.None;
        gameState = GameState.LOSE;
        defaultLoseButton.Select();
        loseHolder.SetActive(true);
    }
}
