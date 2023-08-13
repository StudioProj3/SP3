using System;
using System.Collections.Generic;

using UnityEngine.Assertions;

// All custom transitions should inherit from this
// class instead of `TransitionBase`
public abstract class Transition<TStateID> :
    TransitionBase<TStateID>
{
    private List<Func<bool>> conditions = new();

    protected Transition(TStateID fromStateID, TStateID toStateID,
        params Func<bool>[] initConditions) :
        base(fromStateID, toStateID)
    {
        conditions.AddRange(initConditions);
    }

    protected void AddCondition(Func<bool> condition)
    {
        conditions.Add(condition);
    }

    public override bool Conditions()
    {
        bool? result = conditions.AllTrue();
        Assert.IsTrue(result != null);

        return (bool)result;
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
}
