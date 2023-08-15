// Multiplies base or modified value based on priority
using UnityEngine;

public class MultiplyModifier : Modifier
{
    public MultiplyModifier(float value, int priority = 0)
        : base(value, priority)
    {
    }

    public override float Modify(float value)
    {
        return value * this.value;
    }

    protected override Modifier CreateClone(float value, int priority)
    {
        return new MultiplyModifier(value, priority);
    }
}
