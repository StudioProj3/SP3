using System;

using UnityEngine;

using Newtonsoft.Json;

[Serializable]
public class BoundedValue : IValue
{
    [JsonIgnore]
    public float Max => _maxValueContainer?.Value ?? 0; 

    [JsonIgnore]
    public float Min => _minValueContainer?.Value ?? 0;

    public event Action ValueChanged; 

    [JsonIgnore]
    public float Value
    { 
        get => _value;
        set
        {
            _value = Mathf.Clamp(value, _minValueContainer.Value,
                _maxValueContainer.Value);
            ValueChanged?.Invoke();
        }
    }

    [JsonProperty]
    private IValue _maxValueContainer;
    [JsonProperty]
    private IValue _minValueContainer;


    // Internal bounded value
    [JsonProperty]
    private float _value;

    public void SetMax(ModifiableValue newMax)
    {
        _maxValueContainer = newMax;
    }

    public BoundedValue()
    {
        _value = 0;
    }

    public BoundedValue(IValue minContainer) :
        this(minContainer, null)
    {

    }

    public BoundedValue(IValue minContainer,
        IValue maxContainer)
    {
        _minValueContainer = minContainer;
        _maxValueContainer = maxContainer;
        _value = maxContainer.Value;
    }

    public void OnMaxValueChanged()
    {
        _value = Mathf.Clamp(_value, Min, Max);
        ValueChanged?.Invoke();
    }

    public object Clone()
    {
        return new BoundedValue(
            _minValueContainer?.Clone() as IValue,
            _maxValueContainer?.Clone() as IValue);
    }
}
