// Simple interface for object that have a
// name associated with it
public interface INameable<T>
{
    T Name { get; }
}

// A helper interface `INameable` with the generic
// argument defaulted to a common type
public interface INameable :
    INameable<string>
{

}
