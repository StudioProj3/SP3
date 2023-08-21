using UnityEngine;

[CreateAssetMenu(fileName = "NormalRecipe",
    menuName = "Scriptable Objects/Crafting/Recipes/NormalRecipe")]
public class NormalRecipe :
    RecipeBase, ISuccessRate
{
    [field: HorizontalDivider]
    [field: Header("Success Parameters")]

    [field: SerializeField]
    public bool AlwaysSuccess { get; protected set; }

    [field: SerializeField]
    [field: Range(0f, 100f)]
    public float SuccessRate { get; protected set; } = 100f;

    [field: SerializeField]
    public ItemBase Row1Col1 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row1Col2 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row1Col3 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row2Col1 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row2Col2 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row2Col3 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row3Col1 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row3Col2 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row3Col3 { get; protected set; }

    public NormalRecipe()
    {
        Name = "Normal Recipe";
    }
}
