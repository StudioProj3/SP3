using System.Collections.Generic;
using System;

using UnityEngine;


[CreateAssetMenu(fileName = "Stats", menuName = "Scriptable Objects/Stats")]
public class Stats : ScriptableObject
{
    [Serializable]
    private class StatMapEntry
    {
        public StatType type;
        public float initialValue;
    }

    private Dictionary<StatType, IModifiableValue> _stats = new();
    private Dictionary<StatType, IModifiableValue> _instancedStats = new();



}