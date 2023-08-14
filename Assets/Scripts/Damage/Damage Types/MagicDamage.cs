using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage",
    menuName = "Damage/MagicDamage")]
public class MagicDamage : Damage
{
    public static MagicDamage Create(float damage)
    {
        return CreateInstance<MagicDamage>().Init(damage);
    }

    private MagicDamage Init(float damage)
    {
        _damage = damage;
        return this;
    }

    // OnApply is in 'TakeDamage()' implemented from IDamageable
    // damage.OnApply(_playerStats);
    public override void OnApply(IStatContainer entityStats)
    {
        // DR = (1 – (100/(100 + V))); where V equals the value of armour or magic reduction 
        // and DR is the percentage of damage that is reduced.
        if (entityStats.GetStat("MagicResistance").Value > 1)
        {
            float resistance = (1 - (100 / (100 + entityStats.GetStat("MagicResistance").Value))) * 100;
            entityStats.GetStat("Health").Subtract(_damage * ((100 - resistance) / 100));
        }
        else
        {
            entityStats.GetStat("Health").Subtract(_damage);
        }
    }
}
