using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class ModifiableValue : IValue
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

    // TODO - set this up. https://forum.unity.com/threads/tutorial-character-stats-aka-attributes-system.504095/
    public event Action ValueChanged;
    private readonly List<Modifier> _modifiers;
    private bool _isDirty = true;

    private float _initialValue;
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