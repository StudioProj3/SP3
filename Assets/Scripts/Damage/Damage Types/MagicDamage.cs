using UnityEngine;

[CreateAssetMenu(fileName = "Damage",
    menuName = "Scriptable Objects/Damage/MagicDamage")]
public class MagicDamage : Damage
{
    public static MagicDamage Create(float initialDamage)
    {
        return CreateInstance<MagicDamage>().Init(initialDamage);
    }

    private MagicDamage Init(float initialDamage)
    {
        _damage = new ModifiableValue(initialDamage);
        return this;
    }

    // `OnApply` is in `TakeDamage` implemented from `IDamageable`
    // `damage.OnApply(_playerStats)`
    public override void OnApply(IEffectable effectable)
    {
        var entityStats = effectable.EntityStats;

        // DR = (1 – (100/(100 + V))); where V equals the value of
        // armour or magic reduction and DR is the percentage of
        // damage that is reduced
        if (entityStats.GetStat("MagicResistance").Value > 1)
        {
            float resistance = (1 - (100 / (100 + entityStats.
                GetStat("MagicResistance").Value))) * 100;
            float damage = Mathf.Round(_damage.Value *
                ((100 - resistance) / 100));
            entityStats.GetStat("Health").Subtract(damage);

            if (entityStats.TryGetStat("Sanity", out var sanity))
            {
                Modifier maxHealthReduction = Modifier.Plus(
                        Mathf.Round((sanity.Max - sanity.Value) / sanity.Max
                        * damage * -0.5f), 10000);
                        
                entityStats.GetStat("Health").AddModifier(maxHealthReduction);
            }
        }
        else
        {
            entityStats.GetStat("Health").Subtract(
                Mathf.Round(_damage.Value));
        }
    }
}
