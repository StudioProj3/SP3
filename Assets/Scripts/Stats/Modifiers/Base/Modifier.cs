using System;

public abstract class Modifier : ICloneable
{
    // Priority of the modifier (higher = more priority)
    public int Priority => _priority;

    public float Value => _value;

    // Internal immutable value
    protected readonly float _value;
    protected readonly int _priority;

    public bool Permanent { get; protected set; }

    public Modifier(float value, int priority, bool permanent = true)
    {
        _value = value;
        _priority = priority;
        Permanent = permanent;
    }

    public abstract float Modify(float value);

    // Factory functions
    public static Modifier Plus(float value, int priority, bool permanent = true) =>
        new PlusModifier(value, priority, permanent);

    public static Modifier Multiply(float value, int priority, bool permanent = true) =>
        new MultiplyModifier(value, priority, permanent);

    protected abstract Modifier CreateClone(float value,
        int priority, bool peramenent);

    // `ICloneable` implementation
    public virtual object Clone()
    {
        return CreateClone(_value, _priority, Permanent);
    }
}
