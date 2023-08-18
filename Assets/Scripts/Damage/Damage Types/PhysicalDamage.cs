using UnityEngine;

[CreateAssetMenu(fileName = "Damage",
    menuName = "Scriptable Objects/Damage/PhysicalDamage")]
public class PhysicalDamage : Damage
{
    public static PhysicalDamage Create(float damage)
    {
        return CreateInstance<PhysicalDamage>().Init(damage);
    }

    private PhysicalDamage Init(float damage)
    {
        _damage = damage;
        return this;
    }

    // OnApply is in 'TakeDamage()' implemented from IDamageable
    // damage.OnApply(_playerStats);
    public override void OnApply(IEffectable effectable)
    {
        var entityStats = effectable.EntityStats;
        // DR = (1 – (100/(100 + V))); where V equals the value of armour or magic reduction 
        // and DR is the percentage of damage that is reduced.
        if (entityStats.GetStat("Armor").Value > 1)
        {
            float resistance = (1 - (100 / (100 + entityStats.GetStat("Armor").Value))) * 100;
            float damage = Mathf.Round(_damage * ((100 - resistance) / 100));
            entityStats.GetStat("Health").Subtract(damage);
        }
        else
        {
            entityStats.GetStat("Health").Subtract(Mathf.Round(_damage));
        }
    }
}
