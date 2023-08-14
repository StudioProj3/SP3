using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage",
    menuName = "Damage/PhysicalDamage")]
public class PhysicalDamage : Damage
{
    public static PhysicalDamage Create(float damage)
    {
        return CreateInstance<PhysicalDamage>().Init(damage);
    }

    private PhysicalDamage Init(float damage)
    {
        _amount = damage;
        return this;
    }

    // OnApply is in 'TakeDamage()' implemented from IDamageable
    // damage.OnApply(_playerStats);
    // private void OnApply(IStatContainer entityStats)
    public override void OnApply()
    {
         
    }
}
