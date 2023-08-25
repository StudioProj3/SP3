using System;

public abstract class Modifier : ICloneable
{
    // Priority of the modifier (higher = more priority)
    public int Priority => _priority;

    public float Value => _value;

    // Internal immutable value
    protected readonly float _value;
    protected readonly int _priority;

    public Modifier(float value, int priority)
    {
        _value = value;
        _priority = priority;
    }

    public abstract float Modify(float value);

    // Factory functions
    public static Modifier Plus(float value, int priority) =>
        new PlusModifier(value, priority);

    public static Modifier Multiply(float value, int priority) =>
        new MultiplyModifier(value, priority);

    protected abstract Modifier CreateClone(float value,
        int priority);

    // `ICloneable` implementation
    public virtual object Clone()
    {
        return CreateClone(_value, _priority);
    }
}
