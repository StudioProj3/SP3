// Transition that will happen after some time
public class TimedTransition<TStateID> :
    Transition<TStateID>, ITimedTransition<TStateID>
{
    private bool _isTimeUp = false;
    private readonly float _duration;

    public TimedTransition(TStateID fromStateID, TStateID toStateID,
        float duration) : base(fromStateID, toStateID)
    {
        _duration = duration;
    }

    public void StartTimer()
    {
        _isTimeUp = false;

        // Change `isTimeUp` after `time` to transit
        _ = Delay.Execute(() => _isTimeUp = true, _duration);
    }

    public override bool Conditions()
    {
        return _isTimeUp;
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
