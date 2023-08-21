using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(Damage damageType, Vector3 knockback);
}