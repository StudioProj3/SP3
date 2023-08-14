using System.Collections.Generic;
using System;
using System.Linq;

using UnityEngine;
using UnityEditor;

// NOTE (Chris): Take note that the inspector for this script is in CustomStatsEditor.cs
[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Serializable]
    private class StatMapEntry
    {
        public StatType type;
        public float initialValue;

        public virtual KeyValuePair<StatType, IModifiableValue> CreatePair()
        {
            return new KeyValuePair<StatType, IModifiableValue>(
                type,
                new ModifiableValue(initialValue));
        }
    }

    [Serializable]
    private class BoundedStatMapEntry : StatMapEntry
    {
        // TODO (Chris): Have the user be able to choose between readonly 
        // and not lower bound.
        public float initialLowerBoundValue;

        public override KeyValuePair<StatType, IModifiableValue> CreatePair()
        {
            ModifiableValue modifiableValue = new(initialValue);
            return new KeyValuePair<StatType, IModifiableValue>(type,
                new BoundedModifiableValue(
                    new BoundedValue(
                        new ReadOnlyValueContainer(initialLowerBoundValue),
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
    private Dictionary<StatType, IModifiableValue> _instancedStats = new();

    private readonly Dictionary<string, StatType> _statTypeMap = new();

    private IModifiableValue GetStat(string typeName)
    {
        return _stats[_statTypeMap[typeName]];
    }

    private IModifiableValue GetStat(StatType type)
    {
        return _stats[type];
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
            }
            _statTypeMap.Add(statPair.Key.name, statPair.Key);
        });

        _instancedStatInitializerList.ForEach(stat => 
        {
            var statPair = stat.CreatePair();
            if (statPair.Key != null && statPair.Value != null)
            {
                _instancedStats.Add(statPair.Key, statPair.Value);
            }
            _statTypeMap.Add(statPair.Key.name, statPair.Key);
        });
    }

    // Creates a new instanced dictionary of runtime stats.
    private Dictionary<string, IModifiableValue> GetInstancedStats()
    {
        return _instancedStats.ToDictionary(
            kv => kv.Key.Name, kv => kv.Value.Clone() as IModifiableValue);
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