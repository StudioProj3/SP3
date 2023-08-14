using System.Collections.Generic;
using System;

using UnityEngine;
using System.Xml.Serialization;

// NOTE (Chris): Take note that the inspector for this script is in CustomStatsEditor.cs
[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Serializable]
    private class StatMapEntry
    {
        public StatType type;
        public float initialValue;
    }

    [Serializable]
    private class BoundedStatMapEntry : StatMapEntry
    {
        public float initialLowerBoundValue;
    }

    [SerializeReference]
    private List<StatMapEntry> _statInitializerList;

    [SerializeReference]
    private List<StatMapEntry> _instancedStatInitializerList;

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