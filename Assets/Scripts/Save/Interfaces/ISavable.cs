public interface ISavable :
    ISerializable, IDeserializable
{
    bool EnableSave { get; }

    string SaveID { get; }

    void HookEvents();

    string Save();

    void Load(string data);
}
