using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public interface IEffectable : IDamageable
{
    public IStatContainer EntityStats { get;}
    public void ApplyEffect(StatusEffectBase statusEffect);
    public void RemoveEffect(StatusEffectBase statusEffect);
}
