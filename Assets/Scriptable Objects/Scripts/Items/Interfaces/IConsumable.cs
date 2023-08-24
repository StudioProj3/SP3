using System.Collections.Generic;

using UnityEngine;

public interface IConsumable
{
    void ApplyConsumptionEffect(Stats entityStats, IEffectable effectable);

    void RemoveConsumptionEffect(Stats entityStats, 
        Pair<StatType, Modifier> statTypePair);
}
