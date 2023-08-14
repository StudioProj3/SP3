public interface IModifiableValue : IValue
{
    void AddModifier(Modifier modifier);
    void RemoveModifier(Modifier modifier);
}