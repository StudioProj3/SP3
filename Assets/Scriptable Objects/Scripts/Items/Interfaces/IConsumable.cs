using System.Collections.Generic;

using UnityEngine;

public interface IConsumable
{
    void ApplyConsumptionEffect(Stats entityStats);

    void RemoveConsumptionEffect(Stats entityStats, 
        Pair<StatType, Modifier> statTypePair);
}
