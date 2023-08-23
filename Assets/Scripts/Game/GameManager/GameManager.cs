using System;
using UnityEngine;
public class GameManager : Singleton<GameManager>
{

    public GameState CurrentState;

    public event Action<GameState> OnGameStateChanged;

    private void Start()
    {
        // NOTE (Chris): For debugging purposes, game state will be play.
        ChangeGameState(GameState.Play);
        // ChangeGameState(GameState.MainMenu);
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
