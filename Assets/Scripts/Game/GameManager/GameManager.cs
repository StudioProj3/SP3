using System;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{

    public GameState CurrentState;

    public event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        ChangeGameState(GameState.MainMenu);
    }

    public void ChangeGameState(GameState nextState)
    {
        CurrentState = nextState;

        switch (nextState)
        {
            case GameState.MainMenu:
                break;
            case GameState.Play:
                break;
            case GameState.Pause:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                HandleLoseState();
                break;
        }


        OnGameStateChanged?.Invoke(nextState);
    }

    private void HandleLoseState()
    {
        Debug.Log("Respawn");
    }
}

public enum GameState
{
    MainMenu,
    Play,
    Pause,
    Win,
    Lose,
}
