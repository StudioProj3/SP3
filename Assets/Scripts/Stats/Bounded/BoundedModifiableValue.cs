using System;

public class BoundedModifiableValue :
    IModifiableValue
{
    public float Value => _boundedValue.Value;
    public event Action ValueChanged;

    private readonly BoundedValue _boundedValue;
    private readonly ModifiableValue _modifiableValue;

    public void Add(float toAdd)
    {
        _boundedValue.Value += toAdd;
        ValueChanged?.Invoke();
    }

    public void Subtract(float toSubtract)
    {
        _boundedValue.Value -= toSubtract;
        ValueChanged?.Invoke();
    }

    public void Multiply(float toMultiply)
    {
        _boundedValue.Value *= toMultiply;
        ValueChanged?.Invoke();
    }

    public void Divide(float toDivide)
    {
        _boundedValue.Value /= toDivide;
        ValueChanged?.Invoke();
    }

    public BoundedModifiableValue(BoundedValue value,
        ModifiableValue modifiableValue)
    {
        _boundedValue = value;
        _modifiableValue = modifiableValue;
    }

    public void AddModifier(Modifier modifier)
    {
        _modifiableValue.AddModifier(modifier);
        ValueChanged?.Invoke();
    }

    public void RemoveModifier(Modifier modifier)
    {
        _modifiableValue.RemoveModifier(modifier);
        ValueChanged?.Invoke();
    }

    public object Clone()
    {
        return new BoundedModifiableValue(
            _boundedValue.Clone() as BoundedValue,
            _modifiableValue.Clone() as ModifiableValue);
    }
}
