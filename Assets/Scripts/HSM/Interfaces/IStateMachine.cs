// Used to tell apart a regular state from
// a state machine and also provides functionality
// only present in a state machine
public interface IStateMachine<TStateID>
{
    // Get the current state this state machine is in
    StateBase<TStateID> CurrentState { get; }
}

// A helper interface `IStateMachine` with the generic
// argument defaulted to a common type
public interface IStateMachine :
    IStateMachine<string>
{

}
