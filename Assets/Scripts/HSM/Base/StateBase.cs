using static DebugUtils;

// Base abstract class shared between
// an actual `State` and a `StateMachine`
public abstract class StateBase<TStateID> :
    SMChild<TStateID>, ISealable
{
    public readonly TStateID StateID;

    public abstract bool IsSealed { get; protected set; }

    protected StateBase(TStateID stateID)
    {
        Assert(stateID != null,
            "`stateID` is null");

        StateID = stateID;
    }

    public virtual void Enter()
    {

    }

    public virtual void Update()
    {

    }

    public virtual void FixedUpdate()
    {

    }

    public virtual void LateUpdate()
    {

    }

    public virtual void Exit()
    {

    }

    public abstract void Seal();
}

// A helper abstract class `StateBase` with the generic
// argument defaulted to a common type
public abstract class StateBase : StateBase<string>
{
    protected StateBase(string stateID) :
        base(stateID)
    {

    }
}
