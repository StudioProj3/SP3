using System;
using System.Collections.Generic;

// Use of eager transitions need to be done in caution, as
// careless usage can lead to infinite loops where 2 or more
// states eagerly transit to each other forming a closed cycle
//
// SAFETY: Long eager transition loops cannot be relied upon
// with the current implementation (with recursion) as it will
// stack overflow
public class EagerUnconditionalTransition<TStateID> :
    UnconditionalTransition<TStateID>, IEagerTransition
{
    private List<Func<bool>> _eagerConditions = new();

    public EagerUnconditionalTransition(TStateID fromStateID,
        TStateID toStateID) :
        base(fromStateID, toStateID)
    {

    }

    public EagerUnconditionalTransition(TStateID fromStateID,
        TStateID toStateID, Action callback) :
        this(fromStateID, toStateID)
    {
        SetCallback(callback); 
    }

    public void AddEagerCondition(Func<bool> eagerCondition)
    {
        _eagerConditions.Add(eagerCondition);
    }

    public bool IsEager()
    {
        bool? result = _eagerConditions.AllTrue();

        // Take empty `eagerConditions` (`result` is null)
        // as always true
        return result == null || (bool)result;
    }
}

// A helper class `EagerUnconditionalTransition` with
// the generic argument defaulted to a common type
public class EagerUnconditionalTransition :
    EagerUnconditionalTransition<string>
{
    public EagerUnconditionalTransition(string fromStateID,
        string toStateID) :
        base(fromStateID, toStateID)
    {

    }

    public EagerUnconditionalTransition(string fromStateID,
        string toStateID, Action callback) :
        base(fromStateID, toStateID, callback)
    {

    }
}
