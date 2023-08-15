public class ReadOnlyValueContainer : IValue
{
    private readonly float _value;

    public ReadOnlyValueContainer(float val = 0)
    {
        _value = val;
    }

    public float Value => _value;

    public object Clone()
    {
        return new ReadOnlyValueContainer(_value);
    }
}
