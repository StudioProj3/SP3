using UnityEngine;

public abstract class ItemBase :
    ScriptableObject, INameable, ISellable
{
    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public Sprite Sprite { get; protected set; }

    [field: SerializeField]
    public bool Stackable { get; protected set; }

    [field: SerializeField]
    public float Weight { get; protected set; } = 0f;

    [TextArea]
    public string Description;

    [field: SerializeField]
    public int Cost { get; protected set; } = 0;

}
