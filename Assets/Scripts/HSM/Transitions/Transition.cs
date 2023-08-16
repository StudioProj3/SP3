using System;
using System.Collections.Generic;

using static DebugUtils;

// All custom transitions should inherit from this
// class instead of `TransitionBase`
public abstract class Transition<TStateID> :
    TransitionBase<TStateID>
{
    private List<Func<bool>> _conditions = new();

    private Action _callback;

    public override void SetCallback(Action callback)
    {
        _callback = callback;
    }

    // Should not be called outside of `StateMachine`
    public override void TriggerCallback()
    {
        _callback?.Invoke();
    }

    public override bool Conditions()
    {
        bool? result = _conditions.AllTrue();
        Assert(result != null, "`result` is null");

        return (bool)result;
    }

    protected Transition(TStateID fromStateID, TStateID toStateID,
        params Func<bool>[] initConditions) :
        base(fromStateID, toStateID)
    {
        _conditions.AddRange(initConditions);
    }

    protected Transition(TStateID fromStateID, TStateID toStateID,
        Action callback, params Func<bool>[] initConditions) :
        this(fromStateID, toStateID, initConditions)
    {
        SetCallback(callback);
    }

    protected void AddCondition(Func<bool> condition)
    {
        _conditions.Add(condition);
    }
}

// A helper abstract class `Transition` with the generic
// argument defaulted to a common type
public abstract class Transition :
    Transition<string>
{
    protected Transition(string fromStateID, string toStateID,
        params Func<bool>[] initConditions) :
        base(fromStateID, toStateID, initConditions)
    {

    }

    protected Transition(string fromStateID, string toStateID,
        Action callback, params Func<bool>[] initConditions) :
        base(fromStateID, toStateID, callback, initConditions)
    {

    }
}
