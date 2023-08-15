using System;

public abstract class Modifier : ICloneable
{
    // Priority of the modifier (higher = more priority)
    public int Priority => priority;

    // Internal immutable value
    protected readonly float value;
    protected readonly int priority;

    public Modifier(float value, int priority)
    {
        this.value = value;
        this.priority = priority;
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
        return CreateClone(value, priority);
    }
}
