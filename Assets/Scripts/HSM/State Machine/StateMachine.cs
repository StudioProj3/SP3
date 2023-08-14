using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

// `TSelfID` is used as a type for when this `StateMachine` is
// used as a state inside another `StateMachine` in a HSM setup
// `TStateID` is used as a type for all states in this `StateMachine`
public class StateMachine<TSelfID, TStateID> :
    StateBase<TSelfID>, IStateMachine<TStateID>
{
    public enum TransitionProtocol
    {
        // Handle before message methods
        HandleBefore,

        // Handle after message methods
        HandleAfter,
    }

    public TransitionProtocol CurrentTransitionProtocol
        { get; private set; } = TransitionProtocol.HandleAfter;

    // Store the current state this `StateMachine` is in
    // right now
    public StateBase<TStateID> CurrentState { get; private set; }

    // This state machine is the root state machine if and
    // only if it does not have a parent
    public bool IsRoot => parentStateMachine != null;

    // Dictionary with all states in this state machine (this can
    // contain state machines if it is a HSM)
    private Dictionary<TStateID, StateBase<TStateID>> _allStates = new();

    // Dictionary with all transitions in this state machine
    private Dictionary<TStateID, List<TransitionBase<TStateID>>>
        _allTransitions = new();

    // Stores the state this state macine is in at the start
    private TStateID _startStateID;

    // Max number of exceptions thrown when current state is null,
    // to prevent flooding the editor when message method calls
    // are attempted in loop
    private uint _currentStateExceptionThreshold = 10;

    // Forward the `selfID` to the readonly `StateID`
    // in `StateBase`
    //
    // Constructor that allows adding any amount of states
    // or transitions during initialization
    public StateMachine(TSelfID selfID,
        params SMChild<TStateID>[] initChilds) : base(selfID)
    {
        AddChilds(initChilds);        
    }

    public void SetTransitionProtocol(TransitionProtocol protocol)
    {
        CurrentTransitionProtocol = protocol;
    }

    public void SetStartState(TStateID stateID)
    {
        VerifyID(stateID);

        _startStateID = stateID;
    }

    public void AddState(StateBase<TStateID> newState)
    {
        TStateID stateID = newState.StateID;

        // Validate the new state's ID
        VerifyNewStateID(stateID);

        // Set parent state machine to `this`
        newState.parentStateMachine = this;

        // Add `newState` into `allStates` dictionary
        _allStates.Add(stateID, newState);
    }

    public void AddTransition(TransitionBase<TStateID> newTransition)
    {
        TStateID fromID = newTransition.FromStateID;
        TStateID toID = newTransition.ToStateID;

        // Check that both the from and to state IDs are valid
        VerifyID(fromID, toID);

        bool keyExists = _allTransitions.TryGetValue(fromID, out var list);

        // `fromID` key already exists in the dictionary and `list`
        // is not null
        if (keyExists && list != null)
        {
            _allTransitions[fromID].Add(newTransition);
        }
        // `fromID` key already exists in the dictionary and `list`
        // is null
        else if (keyExists && list == null)
        {
            _allTransitions[fromID] = new List<TransitionBase<TStateID>>
                { newTransition };
        }
        // `fromID` key does not yet exist, add the key entry to the
        // dictionary together with `newTransition`
        else
        {
            _allTransitions.Add(fromID, new List<TransitionBase<TStateID>>
                { newTransition });
        }
    }

    // Add any amount of states and transitions
    public void AddChilds(params SMChild<TStateID>[] childs)
    {
        foreach (SMChild<TStateID> child in childs)
        {
            if (child is StateBase<TStateID> state)
            {
                AddState(state);
            }
            else if (child is TransitionBase<TStateID> transition)
            {
                AddTransition(transition);
            }
        }
    }

    public override void Enter()
    {
        Assert.IsTrue(_startStateID != null);

        // Set current state to the default start state
        CurrentState = _allStates[_startStateID];

        HandleTimedTransitions(CurrentState.StateID);
    }

    public override void Update()
    {
        if (VerifyCurrentState())
        {
            return;
        }

        // Call current state's message method and handle the
        // necessary transitions
        TransitionProtocolHelper(
            () => CurrentState?.Update(),
            Update);
    }

    public override void FixedUpdate()
    {
        if (VerifyCurrentState())
        {
            return;
        }

        // Call current state's message method and handle the
        // necessary transitions
        TransitionProtocolHelper(
            () => CurrentState?.FixedUpdate(),
            FixedUpdate);
    }

    public override void LateUpdate()
    {
        if (VerifyCurrentState())
        {
            return;
        }

        // Call current state's message method and handle the
        // necessary transitions
        TransitionProtocolHelper(
            () => CurrentState?.LateUpdate(),
            LateUpdate);
    }

    public override void Exit()
    {

    }

    private void SwitchState(TStateID toStateID,
        Action eagerCallback = null)
    {
        CurrentState?.Exit();
        
        CurrentState = VerifyGetState(toStateID);

        CurrentState?.Enter();

        HandleTimedTransitions(CurrentState.StateID);

        // Execute the eager callback
        eagerCallback?.Invoke();
    }

    private void HandleTransitions(Action eagerCallback)
    {
        (TStateID, bool) result = TryAllTransitions(CurrentState.StateID);

        // If `result` is default, nothing
        // more needs to be done, return
        if (result.Item1.IsDefault())
        {
            return;
        }

        // Perform the actual transition with the
        // `eagerCallback` where appropriate
        SwitchState(result.Item1,
            result.Item2 ? eagerCallback : null);
    }

    private void HandleTimedTransitions(TStateID stateID)
    {
        bool result = _allTransitions.TryGetValue(stateID, out var list);
        if (!result || list == null || list.Count == 0)
        {
            return;
        }

        // For all timed transitions of `stateID` start the
        // timer
        foreach (TransitionBase<TStateID> transition in list)
        {
            if (transition is ITimedTransition<TStateID> timedTransition)
            {
                timedTransition.StartTimer();
            }
        }
    }

    // Returns the new state the state machine should be in
    // upon taking a transition if a suitable one is found
    // together with whether it is an eager transition with
    // conditions met, else it will return `default(TStateID)`
    private (TStateID, bool) TryAllTransitions(TStateID stateID)
    {
        bool result = _allTransitions.TryGetValue(stateID, out var list);

        // The key `stateID` does not exist or the `list` is null,
        // meaning there are no transitions present for state `stateID`
        if (!result || list == null)
        {
            return (default, false);
        }

        // Loop through all the available transitions and if list
        // is empty this will be skipped and directly returns
        // `default(TStateID)`
        foreach (TransitionBase<TStateID> transition in list)
        {
            // If all transition conditions are met,
            // return the target state
            if (transition.Conditions())
            {
                return (transition.ToStateID,
                    transition is IEagerTransition &&
                    (transition as IEagerTransition).IsEager());
            }
        }

        return (default, false);
    }

    // Checks that the current state is not null,
    // with extra exception threshold to prevent long
    // list of exceptions
    // Returns whether to ignore further execution in
    // the respective message methods
    private bool VerifyCurrentState()
    {
        if (_currentStateExceptionThreshold > 0)
        {
            bool currentState = CurrentState == null;
            if (currentState)
            {
                _currentStateExceptionThreshold--;
            }

            Assert.IsTrue(!currentState);
            return false;
        }

        return true;
    }

    // New state ID validation
    private void VerifyNewStateID(TStateID newStateID)
    {
        Assert.IsTrue(newStateID != null && !newStateID.IsDefault(),
            "`newStateID` must not be null and cannot " +
            "be equals to its default value");
    }

    // Assert that every `stateID` exists in the
    // `allStates` dictionary
    private void VerifyID(params TStateID[] stateIDs)
    {
        foreach (TStateID stateID in stateIDs)
        {
            Assert.IsTrue(_allStates.ContainsKey(stateID));
        }
    }

    // Assert that `stateID` exists in the `allStates` dictionary
    // and return the state if it exists
    private StateBase<TStateID> VerifyGetState(TStateID stateID)
    {
        bool result = _allStates.TryGetValue(stateID, out var state);
        Assert.IsTrue(result);

        return state;
    }

    // Help reduce duplication of `CurrentTransitionProtocol`
    // handlers in the respective message methods
    private void TransitionProtocolHelper(Action messageMethod,
        Action eagerCallback)
    {
        if (CurrentTransitionProtocol ==
            TransitionProtocol.HandleBefore)
        {
            HandleTransitions(eagerCallback);
            messageMethod();
        }
        else if (CurrentTransitionProtocol ==
            TransitionProtocol.HandleAfter)
        {
            messageMethod();
            HandleTransitions(eagerCallback);
        }
    }
}

// A helper class `StateMachine` with generic arguments
// defaulted to a common type
public class StateMachine : StateMachine<string, string>
{
    // Forward the string state id to the readonly
    // `StateID` in `StateBase` through `StateMachine`
    //
    // Constructor that allows adding any amount of states
    // or transitions during initialization
    public StateMachine(string selfID,
        params SMChild<string>[] initChilds) :
        base(selfID, initChilds)
    {

    }
}
