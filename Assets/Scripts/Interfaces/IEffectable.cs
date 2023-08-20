public interface IEffectable : IDamageable
{
    public IStatContainer EntityStats { get; }

    public void ApplyEffect(StatusEffectBase statusEffect);

    public void RemoveEffect(StatusEffectBase statusEffect);
}
