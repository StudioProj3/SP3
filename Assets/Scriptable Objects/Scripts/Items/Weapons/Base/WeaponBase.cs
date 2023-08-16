using UnityEngine;


public abstract class WeaponBase : ItemBase
{
    [field: SerializeField]
    public Damage WeaponDamageType {get; protected set;}
    
    [field: SerializeField]
    public Stats WeaponStats { get; protected set; }
}
