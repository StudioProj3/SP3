using System;

using UnityEngine.Assertions;

// A generic transition that does not warrant an
// additional custom transition which allows adding
// and checking conditions
public class GenericTransition<TStateID> :
    Transition<TStateID>
{
    public GenericTransition(TStateID fromStateID, TStateID toStateID,
        params Func<bool>[] initConditions) :
        base(fromStateID, toStateID, initConditions)
    {
        Assert.IsTrue(initConditions.Length > 0,
            "There must be at least 1 condition in this transition");
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

// A helper class `GenericTransition` with the generic
// argument defaulted to a common type
public class GenericTransition :
    GenericTransition<string>
{
    public GenericTransition(string fromStateID, string toStateID,
        params Func<bool>[] initConditions) :
        base(fromStateID, toStateID, initConditions)
    {

    }
}
