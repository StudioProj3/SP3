using System;

using UnityEngine.Assertions;

public abstract class TransitionBase<TStateID> :
    SMChild<TStateID>
{
    public TStateID FromStateID { get; protected set; }
    public TStateID ToStateID { get; protected set; }

    public abstract void SetCallback(Action callback);

    public abstract void TriggerCallback();

    public abstract bool Conditions();

    // To allow the helper class at the bottom
    // to perform their own assertions and initialization
    protected TransitionBase()
    {

    }

    protected TransitionBase(TStateID fromStateID, TStateID toStateID)
    {
        Assert.IsTrue(fromStateID != null && toStateID != null);

        FromStateID = fromStateID;
        ToStateID = toStateID;
    }
}

// A helper abstract class `TransitionBase` with the
// generic argument defaulted to a common type
public abstract class TransitionBase :
    TransitionBase<string>
{
    protected TransitionBase(string fromStateID, string toStateID)
    {
        // Ensure that the strings passed in is neither null nor empty
        Assert.IsTrue(!string.IsNullOrEmpty(fromStateID) &&
            !string.IsNullOrEmpty(toStateID));

        FromStateID = fromStateID;
        ToStateID = toStateID;
    }
}
