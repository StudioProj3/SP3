using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "StatusEffect/DamageOverTimeEffect")]
public class DamageOverTimeEffect : StatusEffectBase
{
    public float dotAmount;
}
