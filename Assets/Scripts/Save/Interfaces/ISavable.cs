public interface ISavable<T> :
    ISerializable
{
    bool EnableSave { get; }

    string SaveID { get; }

    void HookEvents();

    string Save();

    void Load(string data);
}
