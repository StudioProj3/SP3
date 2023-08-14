using UnityEngine;

public abstract class ItemBase :
    ScriptableObject, INameable
{
    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public Sprite Sprite { get; protected set; }

    [TextArea]
    public string Description;
}
