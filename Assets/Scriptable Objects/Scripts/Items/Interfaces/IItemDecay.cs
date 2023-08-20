public interface IItemDecay
{
    enum DecayDuration
    {
        Fixed,
        Random,
    }

    DecayDuration DurationType { get; }

    float FixedDuration { get; }

    float MinDuration { get; }

    float MaxDuration { get; }

    ItemBase Target { get; }
}
