using UnityEngine;

public abstract class ItemBase :
    ScriptableObject, INameable
{
    [field: HorizontalDivider]
    [field: Header("Basic Parameters")]

    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public Sprite Sprite { get; protected set; }

    [field: SerializeField]
    public bool Atlas { get; protected set; }

    [field: SerializeField]
    public bool Stackable { get; protected set; }

    [field: SerializeField]
    public bool Usable { get; protected set; }

    [field: SerializeField]
    public bool Droppable { get; protected set; }

    [field: SerializeField]
    public float Weight { get; protected set; } = 0f;

    [TextArea]
    public string Description;
}
