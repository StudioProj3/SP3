using System;
using System.Linq;
using System.Collections.Generic;

// Only use this class when needed, since it calculates
// values every get operation.
[Serializable]
public class DynamicModifiableValue : IModifiableValue 
{
    public float Value
    {
        get
        {
            float v = _initialValue;
            foreach (var modifier in _modifiers)
            {
                v = modifier.Modify(v);
            }
            _value = v;

            return _value;
        }
    }

    public IList<Modifier> ModifierList
    {
        get
        {
            IList<Modifier> _modifierList = _modifiers.AsReadOnly();
            return _modifierList;
        }
    }

    public float Max => Value;
    public float Base => _initialValue;

    public IList<Modifier> AppliedModifiers => ModifierList;

    public event Action ValueChanged;

    // List of modifiers sorted on add adn remove
    private List<Modifier> _modifiers;

    // Store the initial value for cloning
    private readonly float _initialValue;

    // The internal value of the container
    private float _value;

    public void Add(float toAdd)
    {
        _value += toAdd;
        ValueChanged?.Invoke();
    }

    public void Subtract(float toSubtract)
    {
        _value -= toSubtract;
        ValueChanged?.Invoke();
    }

    public void Multiply(float toMultiply)
    {
        _value *= toMultiply;
        ValueChanged?.Invoke();
    }

    public void Divide(float toDivide)
    {
        _value /= toDivide;
        ValueChanged?.Invoke();
    }

    public void Set(float toSet)
    {
        _value = toSet;
        ValueChanged?.Invoke();
    }

    public DynamicModifiableValue(float baseValue)
    {
        _initialValue = baseValue;
        _modifiers = new();
    }

    public DynamicModifiableValue(float baseValue, List<Modifier> modifiers)
    {
        _initialValue = baseValue;
        _modifiers = new(modifiers);
    }

    public void AddModifier(Modifier modifier)
    {
        _modifiers.Add(modifier);
        _modifiers = _modifiers.OrderByDescending(m => m.Priority).ToList();

        ValueChanged?.Invoke();
    }
    public void RemoveModifier(Modifier modifier)
    {
        _modifiers.Remove(modifier);
        _modifiers = _modifiers.OrderByDescending(m => m.Priority).ToList();

        ValueChanged?.Invoke();
    }

    public object Clone()
    {
        return new DynamicModifiableValue(_initialValue, _modifiers);
    }

    public void InvokeValueChanged()
    {
        ValueChanged?.Invoke();
    }
}
