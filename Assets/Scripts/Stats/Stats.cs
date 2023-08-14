using System.Collections.Generic;
using System;

using UnityEngine;
using System.Xml.Serialization;

[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Serializable]
    private class StatMapEntry
    {
        public StatType type;
        public float initialValue;
    }

    // [XmlInclude(typeof(StatMapEntry))]
    [Serializable]
    private class BoundedStatMapEntry : StatMapEntry
    {
        public float initialLowerBoundValue;
    }

    [SerializeReference]
    private List<StatMapEntry> _statInitializerList;

    private Dictionary<StatType, IModifiableValue> _stats = new();
    private Dictionary<StatType, IModifiableValue> _instancedStats = new();

    private void OnEnable()
    {

    }

    public void CreateBoundedStat()
    {
        _statInitializerList.Add(new BoundedStatMapEntry());
    }

    public void CreateStat()
    {
        _statInitializerList.Add(new StatMapEntry());
    }

    public void RemoveStatAtIndex(int index)
    {
        _statInitializerList.RemoveAt(index);
    }

}