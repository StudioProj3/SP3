using UnityEngine.Assertions;

// Base abstract class shared between
// an actual `State` and a `StateMachine`
public abstract class StateBase<TStateID> :
    SMChild<TStateID>
{
    public readonly TStateID StateID;

    protected StateBase(TStateID stateID)
    {
        Assert.IsTrue(stateID != null);

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
