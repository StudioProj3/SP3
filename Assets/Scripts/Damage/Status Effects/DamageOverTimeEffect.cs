using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "Scriptable Objects/Status Effect/DamageOverTimeEffect")]
public class DamageOverTimeEffect : StatusEffectBase
{
    [SerializeField]
    protected float _dotAmount;

    private float _currentEffectTime = 0f;
    private float _nextTickTime = 1f;

    public static DamageOverTimeEffect Create(float duration = 0,
        float tickSpeed = 0, float dotAmount = 0)
    {
        return CreateInstance<DamageOverTimeEffect>().
            Init(duration, tickSpeed, dotAmount);
    }

    public static DamageOverTimeEffect Create(DamageOverTimeEffect effect)
    {
        return CreateInstance<DamageOverTimeEffect>().
            Init(effect._duration, effect._tickSpeed, effect._dotAmount);
    }

    public override StatusEffectBase Clone()
    {
        return Create(this);
    }

    private DamageOverTimeEffect Init(float duration,
        float tickSpeed, float dotAmount)
    {
        IsDone = false;

        _duration = duration;
        _tickSpeed = tickSpeed;
        _dotAmount = dotAmount; 

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

        if (_currentEffectTime >= _duration)
        {
            IsDone = true;
        }

        if (_currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _tickSpeed;
            stats.GetStat("Health").Subtract(_dotAmount);
        }
    }
}
