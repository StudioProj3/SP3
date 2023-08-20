using UnityEngine;

public abstract class RecipeBase :
    ScriptableObject, INameable
{
    [field: HorizontalDivider]
    [field: Header("Basic Parameters")]

    [field: SerializeField]
    public string Name { get; protected set; }

    [field: HorizontalDivider]
    [field: Header("Recipe Parameters")]

    [field: SerializeField]
    public ItemBase Target { get; protected set; }

    [field: SerializeField]
    [field: Range(1, 10)]
    public uint TargetQuantity { get; protected set; }
}
