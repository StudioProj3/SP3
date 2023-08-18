using System;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CurrencyCost : ISerializationCallbackReceiver
{
    // NOTE (Chris): Take note of this. Should be global.
    private const int _currencyTypeCount = 3;
    public List<Pair<ItemBase, int>> costs;

    public void OnAfterDeserialize()
    {
        if (costs.Count > _currencyTypeCount)
        {
            Debug.LogErrorFormat("More than {0} costs set.", 
                _currencyTypeCount);
            while (costs.Count > _currencyTypeCount) 
            {
                costs.RemoveAt(costs.Count - 1);
            }
        }
    }

    public void OnBeforeSerialize()
    {
        costs.Capacity = _currencyTypeCount;

        while (costs.Count < costs.Capacity)
        {
            costs.Add(null);
        }
    }
}