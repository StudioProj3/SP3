using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CurrencyCost : ISerializationCallbackReceiver
{
    public List<Pair<ItemBase, int>> costs;

    public void OnAfterDeserialize()
    {
        if (costs.Count > 4)
        {
            Debug.LogError("More than 4 costs set.");
            while (costs.Count > 4) 
            {
                costs.RemoveAt(costs.Count - 1);
            }
        }
    }

    public void OnBeforeSerialize()
    {
        costs.Capacity = 4;

        while (costs.Count < costs.Capacity)
        {
            costs.Add(null);
        }
    }
}