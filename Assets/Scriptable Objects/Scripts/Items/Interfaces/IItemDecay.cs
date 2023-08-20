public interface IItemDecay
{
    enum DecayDuration
    {
        Fixed,
        Random,
    }

    bool Disable { get; }

    DecayDuration DurationType { get; }

    float FixedDuration { get; }

    float MinDuration { get; }

    float MaxDuration { get; }

    ItemBase Target { get; }
}
