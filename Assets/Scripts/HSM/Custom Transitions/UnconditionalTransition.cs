using System;

// Transition that will take place without any conditions
public class UnconditionalTransition<TStateID> :
    Transition<TStateID>
{
    public UnconditionalTransition(TStateID fromStateID,
        TStateID toStateID) :
        base(fromStateID, toStateID)
    {

    }

    public UnconditionalTransition(TStateID fromStateID,
        TStateID toStateID, Action callback) :
        this(fromStateID, toStateID)
    {
        SetCallback(callback);
    }

    public override bool Conditions()
    {
        return true;
    }
}

// A helper class `UnconditionalTransition` with the
// generic argument defaulted to a common type
public class UnconditionalTransition :
    UnconditionalTransition<string>
{
    public UnconditionalTransition(string fromStateID,
        string toStateID) :
        base(fromStateID, toStateID)
    {

    }

    public UnconditionalTransition(string fromStateID,
        string toStateID, Action callback) :
        base(fromStateID, toStateID, callback)
    {

    }
}
