using UnityEngine;

[CreateAssetMenu(fileName = "Bow",
    menuName = "Scriptable Objects/Items/Weapons/Bow")]
public class BowWeaponItem : WeaponBase, IBowWeapon
{
    [field: SerializeField]
    private string _animationName = "Shoot";

    [field: SerializeField]
    private ItemBase _projectile;
    public string AnimationName => _animationName;

    public ItemBase Projectile => _projectile;
    public void OnUseEnter()
    {
        if (CanAttack)
        {
            return;
        }

        _ = Delay.Execute(() =>
        {
            CanAttack = true;
        }, WeaponStats.GetStat("AttackCooldown").Value);
    }

    public void OnUse()
    {
        
    }

    public void OnUseExit()
    {
    }

    public void Shoot(ArrowController projectileToFire, Vector3 direction, Transform source)
    {
        if (CanAttack)
        {
            projectileToFire.Init(direction, WeaponDamageType, WeaponStatusEffect, source, Projectile.Sprite);
            projectileToFire.transform.position = source.position;
            projectileToFire.transform.SetParent(null);
            CanAttack = false;
        }
    }
}
