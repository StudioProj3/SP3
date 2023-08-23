public interface IDeserializable<T>
{
    T Deserialize(string data);
}
