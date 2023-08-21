using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect",
    menuName = "Scriptable Objects/Status Effect/DefenseMultiplierEffect")]

public class DefenseMultiplierEffect : StatusEffectBase
{
    [SerializeField]
    protected float _defenseChange;

    private Modifier _defenseModifier;

    private void OnEnable()
    {
        // NOTE (Brandon): The scriptable object itself is not initialized 
        // with the movement modifier.
    }

    public static DefenseMultiplierEffect Create(float duration = 0,
        float defenseChange = 0)
    {
        return CreateInstance<DefenseMultiplierEffect>().
            Init(duration, defenseChange);
    }

    public static DefenseMultiplierEffect Create(DefenseMultiplierEffect effect)
    {
        return CreateInstance<DefenseMultiplierEffect>().
            Init(effect._duration, effect._defenseChange);
    }

    public override StatusEffectBase Clone()
    {
        return Create(this);
    }

    private DefenseMultiplierEffect Init(float duration,
        float defenseChange)
    {
        IsDone = false;

        _duration = duration;
        _defenseChange = defenseChange;
        _defenseModifier = Modifier.Multiply(defenseChange, 25);

        return this;
    }

    public override void OnApply(IEffectable effectable)
    {
        var stats = effectable.EntityStats;
        stats.GetStat("Armor").AddModifier(_defenseModifier);
        stats.GetStat("MagicResistance").AddModifier(_defenseModifier);


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
        stats.GetStat("Armor").RemoveModifier(_defenseModifier);
        stats.GetStat("MagicResistance").AddModifier(_defenseModifier);

    }
}
