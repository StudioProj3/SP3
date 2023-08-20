using UnityEngine;

[CreateAssetMenu(fileName = "Damage",
    menuName = "Scriptable Objects/Damage/TrueDamage")]
public class TrueDamage : Damage
{
    public static TrueDamage Create(float damage)
    {
        return CreateInstance<TrueDamage>().Init(damage);
    }

    private TrueDamage Init(float damage)
    {
        _damage = damage;
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
            Mathf.Round(_damage));
    }
}
