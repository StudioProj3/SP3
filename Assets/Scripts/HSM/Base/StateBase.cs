using static DebugUtils;

// Base abstract class shared between
// an actual `State` and a `StateMachine`
public abstract class StateBase<TStateID> :
    SMChild<TStateID>, ISealable
{
    public readonly TStateID StateID;

    // Gets whether the current object is already sealed
    public bool IsSealed { get; protected set; }

    // Gets whether the current object is ready to be sealed
    public bool CanSeal { get; protected set; }

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

    public virtual void Seal()
    {
        Assert(CanSeal, "Seal operation is invalid");
        Assert(!IsSealed, "Attempted to reseal");

        IsSealed = true;
    }

    protected abstract void InternalCheckSeal();
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
