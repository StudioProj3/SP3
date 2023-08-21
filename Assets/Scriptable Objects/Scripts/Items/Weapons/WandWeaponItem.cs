using UnityEngine;

[CreateAssetMenu(fileName = "Wand",
    menuName = "Scriptable Objects/Items/Weapons/Wand")]
public class WandWeaponItem : WeaponBase, IMagicWeapon
{
    [SerializeField]
    private string _animationName = "Shoot";

    [SerializeField]
    private ArrowItem _projectile;
    public string AnimationName => _animationName;

    public ArrowItem Projectile => _projectile;

    public float SanityCost => WeaponStats.GetStat("SanityCost").Value;
    public void OnUseEnter()
    {
        if (WeaponDamageType != _projectile.WeaponDamageType ||
            WeaponStatusEffect != _projectile.WeaponStatusEffect)
        {
            WeaponDamageType = _projectile.WeaponDamageType;
            WeaponStatusEffect = _projectile.WeaponStatusEffect;
        }

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

    public override void OnEnable()
    {
        WeaponDamageType = _projectile.WeaponDamageType;
        WeaponStatusEffect = _projectile.WeaponStatusEffect;
    }
}
