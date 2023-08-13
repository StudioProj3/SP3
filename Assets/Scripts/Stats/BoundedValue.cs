using UnityEngine;
using System;

[System.Serializable]
public class BoundedValue : ICloneable
{
    private IValue _maxValueContainer;
    private IValue _minValueContainer;
    
    public float Max => _maxValueContainer?.Value ?? 0; 
    public float Min => _minValueContainer?.Value ?? 0;

    public event Action ValueChanged; 

    public float Value
    { 
        get => _value;
        set
        {
            _value = Mathf.Clamp(value, Min, Max);
            ValueChanged?.Invoke();
        }
    }



    private float _value;

    public BoundedValue(IValue minContainer) : this(minContainer, null) {}

    public BoundedValue(IValue minContainer = null, IValue maxContainer = null)
    {
        this._minValueContainer = minContainer;
        this._maxValueContainer = maxContainer;
        this._value = maxContainer.Value;
    }

    public object Clone()
    {
        return new BoundedValue(_minValueContainer?.Clone() as IValue, _maxValueContainer?.Clone() as IValue);
    }
}