using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI ballsText;
    public TextMeshProUGUI levelText;
    
    public GameObject ballPrefab;
    public GameObject playerPrefab;
    
    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    public GameObject[] levels;
    public static GameManager Instance { get; private set; }

    public enum State
    {
        MENU,
        INIT,
        PLAY,
        LEVELCOMPLETED,
        LOADLEVEL,
        GAMEOVER 
    }

    private int _level;
    public int Level
    {
        get => _level;
        set
        {
            _level = value;
            levelText.text = "LEVEL: " + (_level + 1);
        }
    }

    private int _score;
    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            scoreText.text = "SCORE: " + _score;
        }
    }

    private int _balls;
    public int Balls
    {
        get => _balls;
        set
        {
            _balls = value;
            ballsText.text = "BALLS: " + _balls;
        }
    }

    private State _state;
    private GameObject _currentPlayer;
    private GameObject _currentBall;
    private GameObject _currentLevel;

    private bool _isSwitchingState;


    public void PlayButtonPressed()
    {
        SwitchState(State.INIT);
    }
    
    public void NextLevelButtonPressed()
    {
        SwitchState(State.LOADLEVEL);
    }
    
    public void MainMenuButtonPressed()
    {
        SwitchState(State.MENU);
    }

    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    public void SwitchState(State nextState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(nextState, delay));
    }

    IEnumerator SwitchDelay(State nextState, float delay)
    {
        _isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = nextState;
        BeginState(nextState);
        _isSwitchingState = false;
    }

    private void BeginState(State nextState)
    {
        switch (nextState)
        {
            case State.MENU:
                Cursor.visible = true;
                panelMenu.SetActive(true);
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Level = 0;
                Score = 0;
                Balls = 3;
                _currentPlayer = Instantiate(playerPrefab);
                SwitchState(State.LOADLEVEL);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.PLAY:
                if (_state == State.LEVELCOMPLETED)
                    SwitchState(State.LEVELCOMPLETED);
                break;
            case State.LEVELCOMPLETED:
                Destroy(_currentBall);
                Destroy(_currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                Cursor.visible = true;
                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(true);
                Cursor.visible = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }
    
    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (_currentBall == null)
                {
                    if (Balls > 0)
                    {
                        _currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }
                if (_currentLevel != null && _currentLevel.transform.childCount == 0 && _isSwitchingState == false)
                {
                    SwitchState(State.LEVELCOMPLETED);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
        }
    }
    
    private void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(false);
                panelPlay.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
        }
    }
}
