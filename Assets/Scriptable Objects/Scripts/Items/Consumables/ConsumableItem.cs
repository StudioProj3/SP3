using System.Collections.Generic;
using System.Linq;
using System;

using UnityEngine;


[CreateAssetMenu(fileName = "Consumable",
    menuName = "Scriptable Objects/Items/Consumable")]
public class ConsumableItem : ItemBase, IConsumable
{
    [Serializable]
    private class ConsumptionEffectModifier : ConsumptionEffect
    {
        public int priority;
        public float duration;
    }

    [Serializable]
    private class ConsumptionEffect
    {
        public StatType statToChange;
        public ModifierType modifierType;
        public float modifierValue;
    }

    [SerializeField]
    private List<ConsumptionEffectModifier> _effectsThatModify;

    [SerializeField]
    private List<ConsumptionEffect> _effectsThatAdd;

    private Dictionary<Pair<StatType, Modifier>, float> _statsToModify;
    private Dictionary<StatType, float> _statsToAdd;

    public ConsumableItem()
    {
        _effectsThatModify = new List<ConsumptionEffectModifier>();
    }

    public void ApplyConsumptionEffect(Stats entityStats)
    {
        foreach (KeyValuePair<Pair<StatType, Modifier>, float> statToModify
             in _statsToModify)
        {
            entityStats.GetStat(statToModify.Key.First)
                .AddModifier(statToModify.Key.Second);

            if (statToModify.Value > 0)
            {
                _ = Delay.Execute(() =>
                {
                    RemoveConsumptionEffect
                        (entityStats, statToModify.Key);
                }, statToModify.Value);
            }
        }

        foreach (KeyValuePair<StatType, float> statToAdd in _statsToAdd)
        {
            entityStats.GetStat(statToAdd.Key).Add(statToAdd.Value);
        }
    }

    public void RemoveConsumptionEffect(Stats entityStats, 
        Pair<StatType, Modifier> statTypePair)
    {
        entityStats.GetStat(statTypePair.Key)
            .RemoveModifier(statTypePair.Value);
    }

    private void OnEnable()
    {
        foreach (ConsumptionEffectModifier consumptionEffectModifier
             in _effectsThatModify)
        {
            Modifier modifier;
            if (consumptionEffectModifier.modifierType == ModifierType.Plus)
            {
                modifier = Modifier.Plus(
                        consumptionEffectModifier.modifierValue, 
                        consumptionEffectModifier.priority);
            }
            else if (consumptionEffectModifier.modifierType
                 == ModifierType.Multiply)
            {
                modifier = Modifier.Multiply(
                        consumptionEffectModifier.modifierValue, 
                        consumptionEffectModifier.priority);
            }
            else
            {
                modifier = null;
            }

            _statsToModify = _effectsThatModify
                    .Where(pair => pair != null)
                    .ToDictionary(
                        kv => new Pair<StatType, Modifier>
                            (kv.statToChange, modifier),
                        kv => kv.duration);
        }

        foreach (ConsumptionEffect consumptionEffect in _effectsThatAdd)
        {
            _statsToAdd = _effectsThatAdd
                    .Where(pair => pair != null)
                    .ToDictionary(kv => kv.statToChange,
                                  kv => kv.modifierValue);
        }
    }
}