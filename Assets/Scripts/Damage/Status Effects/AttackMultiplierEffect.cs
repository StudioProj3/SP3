using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "Scriptable Objects/Status Effect/AttackMultiplierEffect")]

public class AttackMultiplierEffect : StatusEffectBase
{
    [SerializeField]
    protected float _attackChange;

    private Modifier _attackModifier;

    private void OnEnable()
    {
        // NOTE (Brandon): The scriptable object itself is not initialized 
        // with the movement modifier.
    }

    public static AttackMultiplierEffect Create(float duration = 0,
        float attackChange = 0)
    {
        return CreateInstance<AttackMultiplierEffect>().
            Init(duration, attackChange);
    }

    public static AttackMultiplierEffect Create(AttackMultiplierEffect effect)
    {
        return CreateInstance<AttackMultiplierEffect>().
            Init(effect._duration, effect._attackChange);
    }

    public override StatusEffectBase Clone()
    {
        return Create(this);
    }

    private AttackMultiplierEffect Init(float duration,
        float attackChange)
    {
        IsDone = false;

        _duration = duration;
        _attackChange = attackChange;
        _attackModifier = Modifier.Multiply(attackChange, 25);

        return this;
    }

    public override void OnApply(IEffectable effectable)
    {
        var stats = effectable.EntityStats;
        stats.GetStat("DamageMultiplier").AddModifier(_attackModifier);


        IsDone = false;
    }

    public override void HandleEffect(IEffectable effectable)
    {
        _ = Delay.Execute(() =>
        {
            IsDone = true;
        }, _duration);
    }

    public override void OnExit(IEffectable effectable)
    {
        var stats = effectable.EntityStats;
        stats.GetStat("DamageMultiplier").RemoveModifier(_attackModifier);
    }
}
