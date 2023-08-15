using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "Status Effect/SpeedMultiplierEffect")]
public class SpeedMultiplierEffect : StatusEffectBase
{
    public float movementPenalty;
}
