// Transition that will happen after some time
public class TimedTransition<TStateID> :
    Transition<TStateID>, ITimedTransition<TStateID>
{
    private bool isTimeUp = false;
    private readonly float duration;

    public TimedTransition(TStateID fromStateID, TStateID toStateID,
        float duration) : base(fromStateID, toStateID)
    {
        this.duration = duration;
    }

    public void StartTimer()
    {
        isTimeUp = false;

        // Change `isTimeUp` after `time` to transit
        _ = Delay.Execute(() => isTimeUp = true, duration);
    }

    public override bool Conditions()
    {
        return isTimeUp;
    }
}

// A helper class `TimedTransition` with the generic argument
// defaulted to a common type
public class TimedTransition :
    TimedTransition<string>, ITimedTransition<string>
{
    public TimedTransition(string fromStateID, string toStateID,
        float duration) : base(fromStateID, toStateID, duration)
    {

    }
}
