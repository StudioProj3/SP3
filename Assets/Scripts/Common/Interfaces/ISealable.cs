// To be implemented with types that can be sealed after
// some amounts of initialization or usage to prevent further
// modifications which can help limit the mutability scope and
// a reduction in tendency for semantic errors
public interface ISealable
{
    bool IsSealed { get; protected set; }

    // Attempts to seal the underlying object
    void Seal();
}
