public interface IStatContainer
{
    IModifiableValue GetStat(string name);
    IModifiableValue GetStat(StatType statType);

    bool TryGetStat(StatType statType, out IModifiableValue stat);
    bool TryGetStat(string name, out IModifiableValue stat);
}