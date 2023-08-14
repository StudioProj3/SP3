using System.Collections.Generic;
using System.Linq;
using System;

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

    public event Action ValueChanged;

    // TODO (Chris): Change to a sorted number. 
    // List of modifiers.
    private readonly List<Modifier> _modifiers;
    private bool _isDirty = true;

    // Store the initial value for cloning.
    private readonly float _initialValue;

    // The internal value of the container.
    private float _value;

    public ModifiableValue(float baseValue)
    {
        this._initialValue = baseValue;
        _modifiers = new();
    }

    public ModifiableValue(float baseValue, List<Modifier> modifiers)
    {
        this._initialValue = baseValue;
        _modifiers = new(modifiers);
    }

    public void AddModifier(Modifier modifier)
    {
        _isDirty = true;
        _modifiers.Add(modifier);
        ValueChanged?.Invoke();
    }

    public void RemoveModifier(Modifier modifier)
    {
        _isDirty = true;
        _modifiers.Remove(modifier);
        ValueChanged?.Invoke();
    }

    public object Clone()
    {
        return new ModifiableValue(_initialValue, _modifiers);
    }
}