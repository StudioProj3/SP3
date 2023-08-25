using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;

// This class should only contain static modifiers
// It caches the internal value to prevent extra calculation
[Serializable]
public class ModifiableValue : IModifiableValue 
{
    public float Value
    {
        get
        {
            if (_isDirty)
            {
                float v = _initialValue;
                _modifiers
                    .OrderByDescending(m => m.Priority)
                    .ToList()
                    .ForEach(m => v = m.Modify(v));
                _isDirty = false;
                _value = v;
            }

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

    // List of modifiers sorted on add and remove
    [JsonProperty]
    private List<Modifier> _modifiers;
    private bool _isDirty = true;

    // Store the initial value for cloning
    [JsonProperty]
    private readonly float _initialValue;

    // The internal value of the container
    [JsonProperty]
    private float _value;

    public void Add(float toAdd)
    {
        _value += toAdd;
        _isDirty = true;
        ValueChanged?.Invoke();
    }

    public void Subtract(float toSubtract)
    {
        _value -= toSubtract;
        _isDirty = true;
        ValueChanged?.Invoke();
    }

    public void Multiply(float toMultiply)
    {
        _value *= toMultiply;
        _isDirty = true;
        ValueChanged?.Invoke();
    }

    public void Divide(float toDivide)
    {
        _value /= toDivide;
        _isDirty = true;
        ValueChanged?.Invoke();
    }

    public void Set(float toSet)
    {
        _value = toSet;
        _isDirty = true;
        ValueChanged?.Invoke();
    }

    public ModifiableValue(float baseValue)
    {
        _initialValue = baseValue;
        _modifiers = new();
        _isDirty = true;
    }

    public ModifiableValue(float baseValue, List<Modifier> modifiers)
    {
        _initialValue = baseValue;
        _modifiers = new(modifiers);
        _isDirty = true;
    }

    public void AddModifier(Modifier modifier)
    {
        _modifiers.Add(modifier);
        _isDirty = true;
        _modifiers = _modifiers.OrderByDescending(m => m.Priority).ToList();

        ValueChanged?.Invoke();
    }
    public void RemoveModifier(Modifier modifier)
    {
        _modifiers.Remove(modifier);
        _isDirty = true;
        _modifiers = _modifiers.OrderByDescending(m => m.Priority).ToList();

        ValueChanged?.Invoke();
    }

    public object Clone()
    {
        return new ModifiableValue(_initialValue, _modifiers);
    }
}
