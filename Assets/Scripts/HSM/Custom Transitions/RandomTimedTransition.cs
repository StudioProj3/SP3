using System;

using UnityEngine;

// Transition that will happen after a random amount of
// seconds where the range will be new each time between
// [minDuration, maxDuration]
public class RandomTimedTransition<TStateID> :
    Transition<TStateID>, ITimedTransition<TStateID>
{
    private bool _isTimeUp = false;
    private readonly float _minDuration;
    private readonly float _maxDuration;

    public RandomTimedTransition(TStateID fromStateID,
        TStateID toStateID, float minDuration, float maxDuration) :
        base(fromStateID, toStateID)
    {
        _minDuration = minDuration;
        _maxDuration = maxDuration;
    }

    public RandomTimedTransition(TStateID fromStateID,
        TStateID toStateID, float minDuration, float maxDuration,
        Action callback) :
        this(fromStateID, toStateID, minDuration, maxDuration)
    {
        SetCallback(callback);
    }

    public void StartTimer()
    {
        _isTimeUp = false;

        float randomDuration =
            UnityEngine.Random.Range(_minDuration, _maxDuration);

        // Change `isTimeUp` after `time` to transit
        _ = Delay.Execute(() => _isTimeUp = true, randomDuration);
    }

    public override bool Conditions()
    {
        return _isTimeUp;
    }
}

// A helper class `RandomTimedTransition` with the
// generic argument defaulted to a common type
public class RandomTimedTransition :
    RandomTimedTransition<string>, ITimedTransition<string>
{
    public RandomTimedTransition(string fromStateID,
        string toStateID, float minDuration, float maxDuration) :
        base(fromStateID, toStateID, minDuration, maxDuration)
    {

    }

    public RandomTimedTransition(string fromStateID,
        string toStateID, float minDuration, float maxDuration,
        Action callback) :
        base(fromStateID, toStateID, minDuration, maxDuration, callback)
    {

    }
}
