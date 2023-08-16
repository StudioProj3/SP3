using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponentBase : UsableComponentBase
{
    [field: SerializeField]
    public Damage WeaponDamageInfo { get; protected set; }

    public override void UseItem() {}
}
