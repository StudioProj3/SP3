using System;

using UnityEngine;

[Serializable]
public class BoundedValue : IValue
{
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

    private readonly IValue _maxValueContainer;
    private readonly IValue _minValueContainer;

    // Internal bounded value
    private float _value;

    public BoundedValue(IValue minContainer) :
        this(minContainer, null)
    {

    }

    public BoundedValue(IValue minContainer = null,
        IValue maxContainer = null)
    {
        _minValueContainer = minContainer;
        _maxValueContainer = maxContainer;
        _value = maxContainer.Value;
    }

    public object Clone()
    {
        return new BoundedValue(
            _minValueContainer?.Clone() as IValue,
            _maxValueContainer?.Clone() as IValue);
    }
}
