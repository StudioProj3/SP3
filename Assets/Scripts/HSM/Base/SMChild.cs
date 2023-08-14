// Abstract class for state machine childs,
// should be inherited by types that can
// be a "child" of a `StateMachine` and needs
// to track their parent (if any)
public abstract class SMChild<TStateID>
{
    public IStateMachine<TStateID> parentStateMachine;

    // Used to query if the state, state machine or
    // transition has a state machine parent
    public bool HasParent => parentStateMachine != null;
}
