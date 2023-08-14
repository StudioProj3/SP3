using System.Collections.Generic;

public class StatContainer : IStatContainer
{
    private Dictionary<StatType, IModifiableValue> _stats;
    private Dictionary<string, StatType> _statTypeMap;

    public StatContainer(Dictionary<StatType, IModifiableValue> stats,
        Dictionary<string, StatType> statTypeMap)
    {
        _stats = stats;
        _statTypeMap = statTypeMap;
    }

    public IModifiableValue GetStat(string typeName)
    {
        return _stats[_statTypeMap[typeName]];
    }

    public IModifiableValue GetStat(StatType statType)
    {
        return _stats[statType];
    }

    public bool TryGetStat(StatType statType, out IModifiableValue stat)
    {
        return _stats.TryGetValue(statType, out stat);
    }

    public bool TryGetStat(string typeName, out IModifiableValue stat)
    {
        stat = null;

        if (_statTypeMap.TryGetValue(typeName, out StatType type))
        {
            return _stats.TryGetValue(type, out stat);
        }

        return false;
    }
}
