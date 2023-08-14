// Transition that will happen after `duration` amount of seconds
// where `duration` is fixed right from the start
public class FixedTimedTransition<TStateID> :
    Transition<TStateID>, ITimedTransition<TStateID>
{
    private bool _isTimeUp = false;
    private readonly float _duration;

    public FixedTimedTransition(TStateID fromStateID, TStateID toStateID,
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

// A helper class `FixedTimedTransition` with the
// generic argument defaulted to a common type
public class FixedTimedTransition :
    FixedTimedTransition<string>, ITimedTransition<string>
{
    public FixedTimedTransition(string fromStateID, string toStateID,
        float duration) : base(fromStateID, toStateID, duration)
    {

    }
}
