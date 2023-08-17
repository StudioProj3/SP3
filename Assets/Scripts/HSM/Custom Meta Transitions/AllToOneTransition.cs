using System;

using static DebugUtils;

// A meta transition that allows the state machine to
// transit from any state except the target to the target
// based on a set of predicates, if live update is true
// If live update is false, the "all" will then refer to
// those states that are added to the state machine before this
// meta transition
public class AllToOneTransition<TStateID> :
    Transition<TStateID>, IMetaTransition
{
    public bool LiveUpdate { get; protected set; } = true;

    public AllToOneTransition(TStateID toStateID,
        params Func<bool>[] initConditions) :
        base(default, toStateID, initConditions)
    {
        Assert(initConditions.Length > 0,
            "There must be at least 1 condition in this transition");
    }

    public AllToOneTransition(TStateID toStateID, Action callback,
        params Func<bool>[] initConditions) :
        base(default, toStateID, callback, initConditions)
    {
        SetCallback(callback);
    }

    public AllToOneTransition(TStateID toStateID,
        bool liveUpdate, params Func<bool>[] initConditions) :
        this(toStateID, initConditions)
    {
        LiveUpdate = liveUpdate;
    }

    public AllToOneTransition(TStateID toStateID, Action callback,
        bool liveupdate, params Func<bool>[] initConditions) :
        this(toStateID, callback, initConditions)
    {
        LiveUpdate = liveupdate;
    }

    public new void AddCondition(Func<bool> condition)
    {
        base.AddCondition(condition);
    }

    public override bool Conditions()
    {
        return base.Conditions();
    }
}

// A helper class `AllToOneTransition` with the generic
// argument defaulted to a common type
public class AllToOneTransition :
    AllToOneTransition<string>
{
    public AllToOneTransition(string toStateID,
        params Func<bool>[] initConditions) :
        base(toStateID, initConditions)
    {

    }

    public AllToOneTransition(string toStateID, Action callback,
        params Func<bool>[] initConditions) :
        base(toStateID, callback, initConditions)
    {

    }

    public AllToOneTransition(string toStateID,
        bool liveUpdate, params Func<bool>[] initConditions) :
        base(toStateID, liveUpdate, initConditions)
    {

    }

    public AllToOneTransition(string toStateID, Action callback,
        bool liveupdate, params Func<bool>[] initConditions) :
        base(toStateID, callback, liveupdate, initConditions)
    {

    }
}
