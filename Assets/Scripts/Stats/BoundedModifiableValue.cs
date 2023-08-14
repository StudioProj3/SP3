public class BoundedModifiableValue : IModifiableValue
{
    public float Value => _boundedValue.Value;

    private readonly BoundedValue _boundedValue;
    private readonly ModifiableValue _modifiableValue;

    public BoundedModifiableValue(BoundedValue value,
        ModifiableValue modifiableValue)
    {
        this._boundedValue = value;
        this._modifiableValue = modifiableValue;
    }

    public void AddModifier(Modifier modifier)
    {
        _modifiableValue.AddModifier(modifier);
    }

    public void RemoveModifier(Modifier modifier)
    {
        _modifiableValue.RemoveModifier(modifier);
    }

    public object Clone()
    {
        return new BoundedModifiableValue(_boundedValue.Clone() as BoundedValue,
            _modifiableValue.Clone() as ModifiableValue);
    }
}
