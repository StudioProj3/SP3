using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using UnityEngine;

// NOTE (Chris): Take note that the inspector for
// this script is in `CustomStatsEditor`
[CreateAssetMenu(fileName = "Stats",
    menuName = "Scriptable Objects/Stats/Stats")]
public class Stats :
    ScriptableObject, IStatContainer, ISavable
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

    [field: HorizontalDivider]
    [field: Header("Save Parameters")]

    [field: SerializeField]
    public bool EnableSave { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public string SaveID { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public ISerializable.SerializeFormat Format
        { get; protected set; }

    private Dictionary<StatType, IModifiableValue> _stats = new();
    private Dictionary<StatType, IModifiableValue> _instancedStats =
        new();

    private readonly Dictionary<string, StatType> _statTypeMap =
        new();

    public void AddListenerToStats()
    {
        if (EnableSave)
        {
            foreach (IModifiableValue value in _stats.Values)
            {
                value.ValueChanged +=
                    () => 
                    {
                        // Debug.Log("Save");
                        SaveManager.Instance.Save(SaveID);
                    };
            }
        }
    }

    public void HookEvents()
    {
        if (EnableSave)
        {
            SaveManager.Instance.Hook(SaveID, Save, Load);
        }
    }

    public string Save()
    {
        ISerializable serializable = this;
        string thisObjStr = serializable.Serialize();

        JsonSerializerSettings settings = new()
        {
            TypeNameHandling = TypeNameHandling.Auto,
        };

        Dictionary<string, IModifiableValue> newDict = new();

        // Remove all temporary modifiers 

        foreach (var pair in _stats)
        {
            newDict.Add(pair.Key.name, pair.Value);
        }

        string statsDictStr = JsonConvert.SerializeObject(newDict,
            typeof(Dictionary<string, IModifiableValue>), settings);

        //List<string> fullStr = new()
            //{ thisObjStr, statsDictStr };

        //return JsonConvert.SerializeObject(fullStr);
        return statsDictStr;
    }

    public void Load(string data)
    {
        //List<string> fullStr = JsonConvert.
        //    DeserializeObject<List<string>>(data);

        // IDeserializable deserializable = this;
        //deserializable.Deserialize(fullStr[0]);
        // deserializable.Deserialize(data);

        //Debug.Log(fullStr);

        JsonSerializerSettings settings = new()
        {
           TypeNameHandling = TypeNameHandling.Objects
        };

        Dictionary<string, IModifiableValue> statsDict =
           JsonConvert.DeserializeObject<Dictionary<
           string, IModifiableValue>>(data, settings);

        Dictionary<StatType, IModifiableValue> newDict = new();
        foreach (var pair in statsDict)
        {
            newDict.Add(Resources.Load<StatType>("Scriptable Objects/Stat Types/" + pair.Key),
                pair.Value);

            if (pair.Value is BoundedModifiableValue value)
            {
                value.BindEvent();
            }
        }

        _stats = newDict;

        // We only want to modify the stat in the saving list
        foreach (var stat in _stats.Values)
        {
            var appliedModifiers = stat.AppliedModifiers;
            List<Modifier> indexesToRemove = new();

            for (int i = 0; i < appliedModifiers.Count; ++i)
            {
                if (!appliedModifiers[i].Permanent)
                {
                    indexesToRemove.Add(appliedModifiers[i]);
                }
            }

            foreach (Modifier modifier in indexesToRemove)
            {
                stat.RemoveModifier(modifier);
            }
        }

        AddListenerToStats();
    }

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
