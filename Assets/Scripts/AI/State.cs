public abstract class State<T>
{
    #region Protected Fields

    protected T _owner; 

    protected FiniteStateMachine<T> _stateMachine; 

    #endregion

    #region Public Functions

    public virtual State<T> SetState(FiniteStateMachine<T> sm, T owner)
    {
        _stateMachine = sm;
        _owner = owner;
        return this;
    }

    public virtual void Enter() {}
    public virtual void Exit() {}

    // NOTE - Update must be overriden
    public abstract void Update();

    #endregion
}