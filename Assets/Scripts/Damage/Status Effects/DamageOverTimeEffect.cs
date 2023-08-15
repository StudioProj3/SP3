using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "StatusEffect/DamageOverTimeEffect")]
public class DamageOverTimeEffect : StatusEffectBase
{
    public float dotAmount;

    private float _currentEffectTime = 0;
    private float _nextTickTime = 1;

    public static DamageOverTimeEffect Create(float duration = 0, float tickSpeed = 0, float dotAmount = 0)
    {
        return CreateInstance<DamageOverTimeEffect>().Init(duration, tickSpeed, dotAmount);
    }

    public static DamageOverTimeEffect Create(DamageOverTimeEffect effect)
    {
        return CreateInstance<DamageOverTimeEffect>().Init(effect.duration, effect.tickSpeed, effect.dotAmount);
    }

    private DamageOverTimeEffect Init(float duration, float tickSpeed, float dotAmount)
    {
        IsDone = false;
        this.duration = duration;
        this.tickSpeed = tickSpeed;
        this.dotAmount = dotAmount; 
        return this;
    }


    public override void OnApply(IEffectable effectable)
    {
        IsDone = false;
        _currentEffectTime = 0;
        _nextTickTime = 1;
    }

    public override void HandleEffect(IEffectable effectable) 
    {
        var stats = effectable.EntityStats;
        _currentEffectTime += Time.deltaTime;

        if (_currentEffectTime >= duration)
        {
            IsDone = true;
        }

        if (_currentEffectTime > _nextTickTime)
        {
            _nextTickTime += tickSpeed;
            stats.GetStat("Health").Subtract(dotAmount);
            Debug.Log("Health = " + stats.GetStat("Health").Value);
        }
    }
}
