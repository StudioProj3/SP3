public interface ISerializable
{
    enum SerializeFormat
    {
        Pretty,
        Minimal,
    }

    SerializeFormat Format { get; }

    string Serialize();
}
