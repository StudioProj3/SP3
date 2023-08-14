using UnityEngine;

public abstract class CharacterDataBase :
    ScriptableObject, INameable
{
    [field: SerializeField]
    public string Name { get; protected set; }
}
