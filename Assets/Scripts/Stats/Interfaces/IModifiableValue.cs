public interface IModifiableValue : IValue
{
    float Max { get; }
    float Base { get; }

    void AddModifier(Modifier modifier);
    void RemoveModifier(Modifier modifier);

    void Add(float toAdd);
    void Subtract(float toSubtract);
    void Multiply(float toMultiply);
    void Divide(float toDivide);

    void Set(float toSet);
}
