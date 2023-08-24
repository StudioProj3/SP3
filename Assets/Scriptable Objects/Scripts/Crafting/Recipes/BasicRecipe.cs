using UnityEngine;

[CreateAssetMenu(fileName = "BasicRecipe",
    menuName = "Scriptable Objects/Crafting/Recipes/BasicRecipe")]
public class BasicRecipe :
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
    public ItemBase Row2Col1 { get; protected set; }

    [field: SerializeField]
    public ItemBase Row2Col2 { get; protected set; }

    public BasicRecipe()
    {
        Name = "Basic Recipe";
    }
}
