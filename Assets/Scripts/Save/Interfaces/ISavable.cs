using System;

public interface ISavable
{
    bool EnableSave { get; }

    string SaveID { get; }

    void HookEvents();

    void Save(object send, EventArgs args);

    void Load(object send, EventArgs args);
}
