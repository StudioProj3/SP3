using System;

using static DebugUtils;

// A meta transition that allows the state machine to
// transit from any state except the target to the target
// based on a set of predicates
public class AllToOneTransition<TStateID> :
    Transition<TStateID>, IMetaTransition
{
    public bool LiveUpdate { get; set; } = true;

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
