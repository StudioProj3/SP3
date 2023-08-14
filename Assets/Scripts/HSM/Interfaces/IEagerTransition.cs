// To be implemented by transitions that are "eager"
// in that once a transition is taken, not only will the
// `Enter` message method be called on the new state, it will
// also call the message method the state machine was in
// when the transition takes place. This allows things to be
// done directly in that message method and not get done the
// next time around (maybe another message method)
//
// Use of eager transitions need to be done in caution, as
// careless usage can lead to infinite loops where 2 or more
// states eagerly transit to each other forming a closed cycle
//
// SAFETY: Long eager transition loops cannot be relied upon
// with the current implementation (with recursion) as it will
// stack overflow
using System;

public interface IEagerTransition
{
    bool IsEager();

    void AddEagerCondition(Func<bool> eagerCondition);
}
