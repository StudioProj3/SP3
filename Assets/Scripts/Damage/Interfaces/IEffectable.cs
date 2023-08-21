public interface IEffectable : IDamageable
{
    IStatContainer EntityStats { get; }

    void ApplyEffect(StatusEffectBase statusEffect);

    void RemoveEffect(StatusEffectBase statusEffect);
}
