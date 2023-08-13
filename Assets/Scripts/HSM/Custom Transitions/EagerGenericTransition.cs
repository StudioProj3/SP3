using System;
using System.Collections.Generic;

// Use of eager transitions need to be done in caution, as
// careless usage can lead to infinite loops where 2 or more
// states eagerly transit to each other forming a closed cycle
//
// SAFETY: Long eager transition loops cannot be relied upon
// with the current implementation (with recursion) as it will
// stack overflow
public class EagerGenericTransition<TStateID> :
    GenericTransition<TStateID>, IEagerTransition
{
    private List<Func<bool>> _eagerConditions = new();

    public EagerGenericTransition(TStateID fromStateID, TStateID toStateID,
        params Func<bool>[] initConditions) :
        base(fromStateID, toStateID, initConditions)
    {

    }

    public bool IsEager()
    {
        bool? result = _eagerConditions.AllTrue();

        // Take empty `eagerConditions` (`result` is null)
        // as always true
        return result == null || (bool)result;
    }

    public void AddEagerCondition(Func<bool> eagerCondition)
    {
        _eagerConditions.Add(eagerCondition);
    }
}

// A helper class `EagerGenericTransition` with the generic
// argument defaulted to a common type
public class EagerGenericTransition :
    EagerGenericTransition<string>
{
    public EagerGenericTransition(string fromStateID, string toStateID,
        params Func<bool>[] initConditions) :
        base(fromStateID, toStateID, initConditions)
    {
        
    }
}
