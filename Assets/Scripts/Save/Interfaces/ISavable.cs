public interface ISavable<T> :
    ISerializable, IDeserializable<T>
{
    bool EnableSave { get; }

    string SaveID { get; }

    void HookEvents();

    string Save();

    void Load(string data);
}
