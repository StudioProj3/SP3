using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "StatusEffect/SpeedMultiplierEffect")]
public class SpeedMultiplierEffect : StatusEffectBase
{
    public float movementPenalty;
}
