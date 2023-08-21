using UnityEngine;

public interface IDamageable
{
    void TakeDamage(Damage damageType, Vector3 knockback);
}
