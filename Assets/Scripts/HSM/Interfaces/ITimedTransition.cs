// Interface that should be implemented by all transitions
// that are timed and require support from the parent state
// machine directly
public interface ITimedTransition<TStateID>
{
    // Start the timed transition timer when the state in
    // question enters
    void StartTimer();
}

// A helper interface `ITimedTransition` with the generic
// argument defaulted to a common type
public interface ITimedTransition :
    ITimedTransition<string>
{

}
