using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;


[CreateAssetMenu(fileName = "Consumable",
    menuName = "Scriptable Objects/Items/Consumable")]
public class ConsumableItem : ItemBase, IConsumable
{
    [Serializable]
    private class ConsumptionEffect
    {
        public StatType statToChange;
        public ModifierType modifierType;
        public float modifierValue;
        public int priority;
    }

    [SerializeField]
    private List<ConsumptionEffect> _statInitializerList;

    private Dictionary<StatType, Modifier> _statsToChange;

    public ConsumableItem()
    {
        _statInitializerList = new List<ConsumptionEffect>();
    }

    public void ApplyConsumptionEffect(Stats _entityStats)
    {
        foreach (KeyValuePair<StatType, Modifier> statToChange in _statsToChange)
        {
            Debug.Log("CHanged stat: " + statToChange.Key +
                        "Modifier: " + statToChange.Value);
            _entityStats.GetStat(statToChange.Key).AddModifier(statToChange.Value);
        }
    }

    private void OnEnable()
    {
        foreach (ConsumptionEffect consumptionEffect in _statInitializerList)
        {
            Modifier modifier;
            if (consumptionEffect.modifierType == ModifierType.Plus)
            {
                modifier = Modifier.Plus(
                        consumptionEffect.modifierValue, 
                        consumptionEffect.priority);
            }
            else if (consumptionEffect.modifierType == ModifierType.Multiply)
            {
                modifier = Modifier.Multiply(
                        consumptionEffect.modifierValue, 
                        consumptionEffect.priority);
            }
            else
            {
                modifier = null;
            }

            _statsToChange = _statInitializerList
                    .Where(pair => pair != null)
                    .ToDictionary(kv => kv.statToChange, kv => modifier);
        }
    }
}