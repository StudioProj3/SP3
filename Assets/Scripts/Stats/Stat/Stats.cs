using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

// NOTE (Chris): Take note that the inspector for
// this script is in `CustomStatsEditor`
[CreateAssetMenu(fileName = "Stats",
    menuName = "Stats/Stats")]
public class Stats :
    ScriptableObject, IStatContainer
{
    [Serializable]
    private class StatMapEntry
    {
        public StatType type;
        public float initialValue;

        public virtual KeyValuePair<StatType, IModifiableValue> CreatePair()
        {
            return new KeyValuePair<StatType, IModifiableValue>(
                type, new ModifiableValue(initialValue));
        }
    }

    [Serializable]
    private class BoundedStatMapEntry : StatMapEntry
    {
        // TODO (Chris): Have the user be able to
        // choose between readonly and not lower bound
        public float initialLowerBoundValue;

        public override KeyValuePair<StatType, IModifiableValue> CreatePair()
        {
            ModifiableValue modifiableValue = new(initialValue);
            return new KeyValuePair<StatType, IModifiableValue>(type,
                new BoundedModifiableValue(
                    new BoundedValue(
                        new ReadOnlyValue(initialLowerBoundValue),
                        modifiableValue
                    ),
                    modifiableValue
                ));
        }
    }

    [SerializeReference]
    private List<StatMapEntry> _statInitializerList;

    [SerializeReference]
    private List<StatMapEntry> _instancedStatInitializerList;

    private Dictionary<StatType, IModifiableValue> _stats = new();
    private Dictionary<StatType, IModifiableValue> _instancedStats =
        new();

    private readonly Dictionary<string, StatType> _statTypeMap =
        new();

    public IModifiableValue GetStat(string typeName)
    {
        return _stats[_statTypeMap[typeName]];
    }

    public IModifiableValue GetStat(StatType type)
    {
        return _stats[type];
    }

    public bool TryGetStat(StatType type, out IModifiableValue stat) 
    {
        return _stats.TryGetValue(type, out stat);
    }

    public bool TryGetStat(string name, out IModifiableValue stat)
    {
        stat = null;

        if (_statTypeMap.TryGetValue(name, out StatType type))
        {
            return _stats.TryGetValue(type, out stat);
        }

        return false;
    }

    private void OnEnable()
    {
        _statTypeMap.Clear();
        _statInitializerList.ForEach(stat => 
        {
            var statPair = stat.CreatePair();
            if (statPair.Key != null && statPair.Value != null)
            {
                _stats.Add(statPair.Key, statPair.Value);
                _statTypeMap.Add(statPair.Key.name, statPair.Key);
            }
        });

        _instancedStatInitializerList.ForEach(stat => 
        {
            var statPair = stat.CreatePair();
            if (statPair.Key != null && statPair.Value != null)
            {
                _instancedStats.Add(statPair.Key, statPair.Value);
                _statTypeMap.Add(statPair.Key.name, statPair.Key);
            }
        });
    }

    // Creates a new instanced stat container of runtime stats
    public StatContainer GetInstancedStatContainer()
    {
        return new StatContainer(_instancedStats.ToDictionary(
            kv => kv.Key, kv => kv.Value.Clone() as IModifiableValue),
            _statTypeMap);
    }

    public void CreateBoundedStat()
    {
        _statInitializerList.Add(new BoundedStatMapEntry());
    }

    public void CreateStat()
    {
        _statInitializerList.Add(new StatMapEntry());
    }

    public void CreateBoundedInstancedStat()
    {
        _instancedStatInitializerList.Add(new BoundedStatMapEntry());
    }

    public void CreateInstancedStat()
    {
        _instancedStatInitializerList.Add(new StatMapEntry());
    }

    public void RemoveStatAtIndex(int index)
    {
        _statInitializerList.RemoveAt(index);
    }

    public void RemoveInstancedStatAtIndex(int index)
    {
        _instancedStatInitializerList.RemoveAt(index);
    }
}
