using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "Scriptable Objects/Status Effect/SpeedMultiplierEffect")]
public class SpeedMultiplierEffect : StatusEffectBase
{
    public float movementPenalty;

    private float _currentEffectTime = 0;
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
        var stats = effectable.EntityStats;
        stats.GetStat("MoveSpeed").AddModifier(_movementModifier);

        IsDone = false;
        _currentEffectTime = 0;
    }

    public override void HandleEffect(IEffectable effectable) 
    {
        _currentEffectTime += Time.deltaTime;

        if (_currentEffectTime >= duration)
        {
            IsDone = true;
        }
    }

    public override void OnExit(IEffectable effectable) 
    {
        var stats = effectable.EntityStats;
        stats.GetStat("MoveSpeed").RemoveModifier(_movementModifier);
    }
}
