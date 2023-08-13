using System;

// A generic state that does not warrant an additional
// custom state class which allows adding and removing
// actions from an instance
public class GenericState<TStateID> : State<TStateID>
{
    // Forward the `stateID` to the readonly `StateID`
    // in `StateBase` through `State`
    //
    // Constructor that allows adding any amount of
    // action entries during initialization
    public GenericState(TStateID stateID,
        params ActionEntry[] initActions) :
        base(stateID, initActions)
    {

    }

    public new void AddAction(params ActionEntry[] entries)
    {
        base.AddAction(entries);
    }

    public new void AddAction(StateMessageMethodFlag method,
        Action action, uint? priority = null)
    {
        base.AddAction(method, action, priority);
    }

    public new void AddAction(string method, Action action,
        uint? priority = null)
    {
        base.AddAction(method, action, priority);
    }

    public new bool RemoveAction(StateMessageMethod method,
        uint priority)
    {
        return base.RemoveAction(method, priority);
    }
}

// A helper class `GenericState` with the generic
// argument defaulted to a common type
public class GenericState : GenericState<string>
{
    // Forward the `stateID` to the readonly `StateID`
    // in `StateBase` through `GenericState`
    //
    // Constructor that allows adding any amount of
    // action entries during initialization
    public GenericState(string stateID,
        params ActionEntry[] initActions) :
        base(stateID, initActions)
    {

    }
}
