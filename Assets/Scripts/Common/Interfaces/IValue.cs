using System;

public interface IValue<T> : ICloneable
{
    T Value { get; }
}

// A helper interface `IValue` with the generic
// argument defaulted to a common type
public interface IValue : IValue<float>
{

}
