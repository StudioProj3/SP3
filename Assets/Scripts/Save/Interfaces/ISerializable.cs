public interface ISerializable
{
    enum SerializeFormat
    {
        Minimal,
        Pretty,
    }

    SerializeFormat Format { get; }

    string Serialize();
}
