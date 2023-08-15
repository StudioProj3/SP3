using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable : IDamageable
{
    public void ApplyEffect(StatusEffectBase statusEffect);
    public void RemoveEffect();

    public void HandleEffect();
}
