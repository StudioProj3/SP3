using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "Scriptable Objects/Status Effect/SpeedMultiplierEffect")]
public class SpeedMultiplierEffect : StatusEffectBase
{
    [SerializeField]
    protected float _movementPenalty;

    private Modifier _movementModifier;

    private void OnEnable()
    {
        // NOTE (Brandon): The scriptable object itself is not initialized 
        // with the movement modifier.
    }

    public static SpeedMultiplierEffect Create(float duration = 0,
        float movementPenalty = 0)
    {
        return CreateInstance<SpeedMultiplierEffect>().
            Init(duration, movementPenalty);
    }

    public static SpeedMultiplierEffect Create(SpeedMultiplierEffect effect)
    {
        return CreateInstance<SpeedMultiplierEffect>().
            Init(effect._duration, effect._movementPenalty);
    }

    public override StatusEffectBase Clone()
    {
        return Create(this);
    }

    private SpeedMultiplierEffect Init(float duration,
        float movementPenalty)
    {
        IsDone = false;

        _duration = duration;
        _movementPenalty = movementPenalty; 
        _movementModifier = Modifier.Multiply(movementPenalty, 25, false);

        return this;
    }

    public override void OnApply(IEffectable effectable)
    {
        var stats = effectable.EntityStats;
        stats.GetStat("MoveSpeed").AddModifier(_movementModifier);
        
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
        stats.GetStat("MoveSpeed").RemoveModifier(_movementModifier);
    }
}
