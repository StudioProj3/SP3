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
        Debug.Assert(stateTypes.Where(t => t.IsSubclassOf(typeof(State<T>)))
            .Count() > 0, "Invalid Type passed into SetState function.");

        if (!_currentStates.IsNullOrEmpty())
        {
            _currentStates.ForEach(s => s.Exit());
            _currentStates.Clear();
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
                    Debug.LogWarningFormat(
                        "Type passed was already passed in. Type name: '{0}'",
                        stateType.Name);
                }
            }
            else
            {
                Debug.LogWarningFormat(
                    "Type passed was not added first. Type name: '{0}'",
                    stateType.Name);
            }
        }
    }

    public void ChangeState<TF, TT>() where TF : State<T> where TT : State<T>
    {
        // TODO (Chris): Review this. Not completely sure if this works.

        // TODO (Chris): URGENT -> We SHOULD change the list into a dictionary so we
        // can have a faster lookup.

        // NOTE (Chris): We want to find the state that is easier to look up
        // first. This ties up with the FIXME above this.
        if (_states.TryGetValue(typeof(TT), out State<T> toState))
        {
            return;
        }

        var fromStatePairEnumerable =
            _currentStates.Select((state, index) =>
                new { state, index = index + 1 })
                .Where(pair => pair.state.GetType() == typeof(TF));

        // This will return -1 if there is no state found that matches.
        var index = fromStatePairEnumerable.Select(pair => pair.index)
                    .FirstOrDefault() - 1;
        
        if (index == -1)
        {
            return;
        }

        State<T> fromState = fromStatePairEnumerable
            .Select(pair => pair.state).FirstOrDefault();
        
        // Now that we have both states, call the enter and exit functions.
        fromState.Exit();

        // Remove the current state and add the new one.
        _currentStates.RemoveAt(index);
        _currentStates.Add(toState);

        toState.Enter();
    }

    #endregion
}