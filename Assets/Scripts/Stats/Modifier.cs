using System;

public enum ModifierType
{
    Plus,
    Multiply
}

public abstract class Modifier : ICloneable
{
    protected readonly float value;
    protected readonly int priority;
    public int Priority => priority;

    public Modifier(float value, int priority)
    {
        this.value = value;
        this.priority = priority;
    }

    public abstract float Modify(float value);

    public static Modifier Plus(float value, int priority)
        => new PlusModifier(value, priority);

    public static Modifier Multiply(float value, int priority)
        => new PlusModifier(value, priority);

    protected abstract Modifier CreateClone(float value, int priority);
    public virtual object Clone()
    {
        return CreateClone(value, priority);
    }
}


