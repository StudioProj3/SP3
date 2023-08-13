using UnityEngine;

public abstract class ItemBase :
    ScriptableObject, INameable
{
    public string Name { get; }
}
