using System;
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
        switch (newState)
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
            
            case GameState.Dialogue:
                break;

            default:
                Fatal("Unhandled `GameState` type");
                break;
        }
    }

    private void HandleLoseState()
    {
        Log("Respawn");
    }
}
