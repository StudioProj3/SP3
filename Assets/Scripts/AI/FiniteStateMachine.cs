using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FiniteStateMachine<T>
{
    #region Private Fields

    private T _owner;
    private Dictionary<Type, State<T>> _states;
    private List<State<T>> _currentStates;

    #endregion

    #region Public Functions

    public void Update()
    {
        _currentStates.ForEach(s => s?.Update());
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

    public void AddState(params State<T>[] states)
    {
        foreach (State<T> state in states)
        {
            state.SetState(this, _owner);
            _states[state.GetType()] = state;
        }
    }

    public void SetState(params Type[] stateTypes)
    {
        Debug.Assert(stateTypes.Where(t => !t.IsSubclassOf(typeof(State<T>))).Count() > 0, "Invalid Type passed into SetState function.");

        if (_currentStates.IsNullOrEmpty())
        {
            _currentStates.ForEach(s => s.Exit());
        }

        foreach (Type stateType in stateTypes)
        {
            if (_states.ContainsKey(stateType))
            {
                State<T> state = _states[stateType];

                // Check if the state already exists.
                if (!_currentStates.Exists(s => s == state))
                {
                    _currentStates.Add(state);
                    state.Enter();
                }
                else
                {
                    Debug.LogWarningFormat("Type passed into SetState was already passed in. Type name: '{0}'", stateType.Name);
                }
            }
            else
            {
                Debug.LogWarningFormat("Type passed into SetState was not added first. Type name: '{0}'", stateType.Name);
            }
        }
    }

    #endregion
}