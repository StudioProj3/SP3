// Multiplies base or modified value based on priority
using UnityEngine;

public class MultiplyModifier : Modifier
{
    public MultiplyModifier(float value, int priority = 0, bool permanent = true)
        : base(value, priority, permanent)
    {
    }

    public override float Modify(float value)
    {
        return value * _value;
    }

    protected override Modifier CreateClone(float value, int priority, bool permanent = true)
    {
        return new MultiplyModifier(value, priority, permanent);
    }
}
