public interface IMetaTransition
{
    // Whether the current meta transition is updated
    // for states added after said meta transition
    bool LiveUpdate { get; }
}
