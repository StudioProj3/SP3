// To be implemented with types that can be sealed after
// some amounts of initialization or usage to prevent further
// modifications which can help limit the mutability scope and
// a reduction in tendency for semantic errors
public interface ISealable
{
    bool IsSealed { get; }

    // Whether the underlying object is ready to be sealed as
    // some objects require mandatory actions upon initialization
    // hence, this is used to check whether the seal can be safely
    // performed
    bool CanSeal { get; }

    // Attempts to seal the underlying object
    void Seal();
}
