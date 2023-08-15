using System;

using UnityEngine;

// Simple pair holding 2 values
// of possibly distinct types
[Serializable]
public class Pair<K, V>
{
    [field: SerializeField]
    public K First { get; set; }

    [field: SerializeField]
    public V Second { get; set; }

    // Allow access via `Key`
    public K Key
    {
        get => First;
        set => First = value;
    }

    // Allow access via `Value`
    public V Value
    {
        get => Second;
        set => Second = value;
    }

    public Pair(K first = default, V second = default)
    {
        First = first;
        Second = second;
    }
}

// A simple helper class where both
// the generic arguments are the same
[Serializable]
public class Pair<T> : Pair<T, T>
{

}

// A simple helper class with the first
// generic argument set to string
[Serializable]
public class PairStr<T> : Pair<string, T>
{

}
