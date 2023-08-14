public interface IModifiableValue : IValue
{
    void AddModifier(Modifier modifier);
    void RemoveModifier(Modifier modifier);

    void Add(float toAdd);
    void Subtract(float toSubtract);
    void Multiply(float toMultiply);
    void Divide(float toDivide);
}
