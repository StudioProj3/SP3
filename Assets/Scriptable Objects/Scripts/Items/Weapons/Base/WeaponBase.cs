using TMPro;
using UnityEngine;

public abstract class WeaponBase : ItemBase
{
    [field: HorizontalDivider]
    [field: Header("Weapon Parameters")]

    [field: SerializeField]
    public Damage WeaponDamageType {get; protected set;}

    [field: SerializeField]
    public StatusEffectBase WeaponStatusEffect {get; protected set;}
    
    [field: SerializeField]
    public Stats WeaponStats { get; protected set; }

    public bool CanAttack { get; protected set; }

    public virtual void OnEnable()
    {
        CanAttack = true;
    }
}
