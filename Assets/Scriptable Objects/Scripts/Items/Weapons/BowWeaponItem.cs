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
        
    }

    public void OnUse()
    {
        
    }

    public void OnUseExit()
    {
        _ = Delay.Execute(() =>
        {
            CanAttack = true;
        }, WeaponStats.GetStat("AttackSpeed").Value);
    }

    public void Shoot(ArrowController projectileToFire, Vector3 direction, Transform source)
    {
        if (CanAttack)
        {
            Debug.Log("Shoot");
            projectileToFire.Init(direction, WeaponDamageType, WeaponStatusEffect, source, Projectile.Sprite);
            projectileToFire.transform.position = source.position;
            projectileToFire.transform.SetParent(null);
            CanAttack = false;
        }
    }
}
