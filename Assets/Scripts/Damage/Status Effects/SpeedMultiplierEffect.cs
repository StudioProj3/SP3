using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "Scriptable Objects/Status Effect/SpeedMultiplierEffect")]
public class SpeedMultiplierEffect : StatusEffectBase
{
    public float movementPenalty;

    private Modifier _movementModifier;

    private void OnEnable()
    {
        // NOTE (Brandon): The scriptable object itself is not initialized 
        // with the movement modifier.
    }

    public static SpeedMultiplierEffect Create(float duration = 0, float movementPenalty = 0)
    {
        return CreateInstance<SpeedMultiplierEffect>().Init(duration, movementPenalty);
    }

    public static SpeedMultiplierEffect Create(SpeedMultiplierEffect effect)
    {
        return CreateInstance<SpeedMultiplierEffect>().Init(effect.duration, effect.movementPenalty);
    }

    public override StatusEffectBase Clone()
    {
        return Create(this);
    }

    private SpeedMultiplierEffect Init(float duration, float movementPenalty)
    {
        IsDone = false;
        this.duration = duration;
        this.movementPenalty = movementPenalty; 
        _movementModifier = Modifier.Multiply(movementPenalty, 25);
        return this;
    }

    public override void OnApply(IEffectable effectable)
    {
        Debug.Log(_movementModifier);
        var stats = effectable.EntityStats;
        stats.GetStat("MoveSpeed").AddModifier(_movementModifier);
        
        IsDone = false;
    }

    public override void HandleEffect(IEffectable effectable) 
    {
        _ = Delay.Execute(() =>
        {
            IsDone = true;
        }, duration);
    }

    public override void OnExit(IEffectable effectable) 
    {

        var stats = effectable.EntityStats;
        stats.GetStat("MoveSpeed").RemoveModifier(_movementModifier);
    }
}
