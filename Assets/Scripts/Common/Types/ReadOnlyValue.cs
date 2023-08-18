public class ReadOnlyValue<T> : IValue<T>
{
    private readonly T _value;

    public ReadOnlyValue(T initValue = default)
    {
        _value = initValue;
    }

    public T Value => _value;

    public object Clone()
    {
        return new ReadOnlyValue<T>(_value);
    }
}

// A helper class `ReadOnlyValue` with the generic
// argument defaulted to a common type
public class ReadOnlyValue : IValue
{
    private readonly float _value;

    public ReadOnlyValue(float initValue = 0f)
    {
        _value = initValue;
    }

    public float Value => _value;

    public object Clone()
    {
        return new ReadOnlyValue(_value);
    }
}
