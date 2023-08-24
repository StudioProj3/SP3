using UnityEngine;

public interface IDeserializable
{
    void Deserialize(string data)
    {
        JsonUtility.FromJsonOverwrite(data, this);
    }
}
