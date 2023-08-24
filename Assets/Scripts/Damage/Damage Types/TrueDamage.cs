using UnityEngine;

[CreateAssetMenu(fileName = "Damage",
    menuName = "Scriptable Objects/Damage/TrueDamage")]
public class TrueDamage : Damage
{
    public static TrueDamage Create(float initialDamage)
    {
        return CreateInstance<TrueDamage>().Init(initialDamage);
    }

    private TrueDamage Init(float initialDamage)
    {
        _damage = new ModifiableValue(initialDamage);
        return this;
    }

    // `OnApply` is in `TakeDamage` implemented from `IDamageable`
    // `damage.OnApply(_playerStats)`
    public override void OnApply(IEffectable effectable)
    {
        var entityStats = effectable.EntityStats;

        // True damage deals damage directly to health,
        // no way to reduce it
        entityStats.GetStat("Health").Subtract(
            Mathf.Round(_damage.Value));

        if (entityStats.TryGetStat("Sanity", out var sanity))
            {
                Modifier maxHealthReduction = Modifier.Plus(
                        Mathf.Round((sanity.Max - sanity.Value) / sanity.Max
                        * _damage.Value * -0.5f), 10000);
                        
                entityStats.GetStat("Health").AddModifier(maxHealthReduction);
            }
    }
}
