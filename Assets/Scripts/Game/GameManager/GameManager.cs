using System;
using UnityEngine;
using static DebugUtils;

public class GameManager : Singleton<GameManager>
{
    public GameState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState == value)
            {
                return;
            }

            _currentState = value;
            OnGameStateChanged?.Invoke(_currentState);
        }
    }

    private GameState _currentState;

    public event Action<GameState> OnGameStateChanged;

    protected override void OnStart()
    {
        OnGameStateChanged += GameStateChangeHandler;

        // NOTE (Chris): For debugging purposes, game state will be play.
        CurrentState = GameState.Play;
        // ChangeGameState(GameState.MainMenu);
    }

    private void GameStateChangeHandler(GameState newState)
    {
        if (newState != GameState.Pause)
        {
            UnPause();
        }

        switch (newState)
        {
            case GameState.MainMenu:
                break;

            case GameState.Play:
                break;

            case GameState.Pause:
                HandlePauseState();
                break;

            case GameState.Win:
                HandleWinState();
                break;

            case GameState.Lose:
                HandleLoseState();
                break;
            
            case GameState.Dialogue:
                break;

            default:
                Fatal("Unhandled `GameState` type");
                break;
        }
    }

    private void HandleLoseState()
    {
        LoadingManager.Instance.LoadSceneAdditive("UIDeathScreen", true);
        LoadingManager.Instance.UnloadScene("UI");
        LoadingManager.Instance.UnloadScene("UIHUD");
        GameStateChangeHandler(GameState.Pause);
    }

    private void HandleWinState()
    {
        LoadingManager.Instance.LoadSceneAdditive("UIWinScreen", true);
        LoadingManager.Instance.UnloadScene("UI");
        LoadingManager.Instance.UnloadScene("UIHUD");
        GameStateChangeHandler(GameState.Pause);
    }

    private void HandlePauseState()
    {
        Time.timeScale = 0;
    }

    private void UnPause()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        //Debug.Log(CurrentState);
    }
}
