using System;
using System.Collections.Generic;

using Newtonsoft.Json;

public class BoundedModifiableValue :
    IModifiableValue
{
    [JsonIgnore]
    public float Max => _modifiableValue.Value;

    [JsonIgnore]
    public float Value => _boundedValue.Value;

    [JsonIgnore]
    public float Base => _modifiableValue.Base;

    public IList<Modifier> AppliedModifiers => _modifiableValue.AppliedModifiers;

    public event Action ValueChanged;

    [JsonProperty]
    private readonly BoundedValue _boundedValue;

    [JsonProperty]
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

    public void Set(float toSet)
    {
        _boundedValue.Value = toSet;
        ValueChanged?.Invoke();
    }

    public BoundedModifiableValue(BoundedValue value,
        ModifiableValue modifiableValue)
    {
        _boundedValue = value;
        _modifiableValue = modifiableValue;
        _modifiableValue.ValueChanged += () => _boundedValue.OnMaxValueChanged();
    }

    public BoundedModifiableValue()
    {

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

    public void BindEvent()
    {
        if (_modifiableValue == null || _boundedValue == null)
        {
            return;
        }

        _modifiableValue.ValueChanged += () => _boundedValue.OnMaxValueChanged();
        _boundedValue.SetMax(_modifiableValue);
    }

    public void InvokeValueChanged()
    {
        _modifiableValue?.InvokeValueChanged();
    }
}
