using System.Collections;
using UnityEngine;

public class SwordWeaponItem : WeaponBase
{
    [field: SerializeField]
    public Sprite WeaponSprite {get; private set;}
    public override void OnUseEnter(IEffectable effectable)
    {
    }
}
