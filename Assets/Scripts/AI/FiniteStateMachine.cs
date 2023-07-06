using System;
using System.Collections.Generic;

public class FiniteStateMachine<T>
{
    #region Private Fields

    private T _owner;
    private Dictionary<Type, State<T>> _states;
    private State<T> _currentState;

    #endregion

    #region Public Functions

    public void Update()
    {
        _currentState?.Update();
    }

    public FiniteStateMachine(T owner)
    {
        _owner = owner;
        _states = new Dictionary<System.Type, State<T>>();
    }

    public void AddState(State<T> state)
    {
        state.SetState(this, _owner);
        _states[state.GetType()] = state;
    }

    public void SetState<S>() where S : State<S>
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        if (_states.ContainsKey(typeof(S)))
        {
            _currentState = _states[typeof(S)];
            _currentState.Enter();
        }
    }

    #endregion
}