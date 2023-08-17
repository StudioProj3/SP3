using UnityEngine;

[CreateAssetMenu(fileName = "Damage",
    menuName = "Scriptable Objects/Damage/ColdDamage")]

    // NOTE (BRANDON): This damage type is redundant,
    // if a weaopn wants to apply status effect,
    // set it in the weapon instead.
public class ColdDamage : Damage
{
    private SpeedMultiplierEffect _speedMultiplierEffect;
    public static ColdDamage Create(float damage, SpeedMultiplierEffect speedMultiplierEffect)
    {
        return CreateInstance<ColdDamage>().Init(damage, speedMultiplierEffect);
    }

    private ColdDamage Init(float damage, SpeedMultiplierEffect speedMultiplierEffect)
    {
        _damage = damage;
        _speedMultiplierEffect = speedMultiplierEffect;
        return this;
    }

    // OnApply is in 'TakeDamage()' implemented from IDamageable
    // damage.OnApply(_playerStats);
    public override void OnApply(IEffectable effectable)
    {
        var entityStats = effectable.EntityStats;
        // DR = (1 – (100/(100 + V))); where V equals the value of armour or magic reduction 
        // and DR is the percentage of damage that is reduced.
        if (entityStats.GetStat("MagicResistance").Value > 1)
        {
            float resistance = (1 - (100 / (100 + entityStats.GetStat("MagicResistance").Value))) * 100;
            float damage = Mathf.Round(_damage * ((100 - resistance) / 100));
            entityStats.GetStat("Health").Subtract(damage);
        }
        else
        {
            entityStats.GetStat("Health").Subtract(Mathf.Round(_damage));
        }

        effectable.ApplyEffect(SpeedMultiplierEffect.Create(_speedMultiplierEffect));
    }
}
