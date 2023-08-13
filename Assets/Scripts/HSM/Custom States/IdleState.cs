// A simple state that does nothing and does
// not allow adding any actions
// This state is normally paired with `TimedTransition`
public class IdleState<TStateID> :
    State<TStateID>
{
    public IdleState(TStateID stateID) :
        base(stateID)
    {

    }
}

// A helper class `IdleState` with the generic
// argument defaulted to a common type
public class IdleState :
    IdleState<string>
{
    public IdleState(string stateID) : base(stateID)
    {

    }
}
