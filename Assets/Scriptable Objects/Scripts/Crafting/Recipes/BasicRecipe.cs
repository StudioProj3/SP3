using UnityEngine;

[CreateAssetMenu(fileName = "BasicRecipe",
    menuName = "Scriptable Objects/Crafting/Recipes/BasicRecipe")]
public class BasicRecipe : RecipeBase
{
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
