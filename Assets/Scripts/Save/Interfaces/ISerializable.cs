using UnityEngine;

public interface ISerializable
{
    enum SerializeFormat
    {
        Minimal,
        Pretty,
    }

    SerializeFormat Format { get; }

    string Serialize()
    {
        return JsonUtility.ToJson(this,
            Format == SerializeFormat.Pretty);
    }
}
