using UnityEditor;

[CustomEditor(typeof(BasicRecipe))]
public class BasicRecipeEditor :
    RecipeEditorBase
{
    public override void OnInspectorGUI()
    {
        BasicRecipe recipe = (BasicRecipe)target;
        _items = new[] {
            new[] { recipe.Row1Col1, recipe.Row1Col2 },
            new[] { recipe.Row2Col1, recipe.Row2Col2 }
        };
        _itemsName = new[] {
            new[] { "Row1Col1", "Row1Col2" },
            new[] { "Row2Col1", "Row2Col2" },
        };

        base.OnInspectorGUI();
    }
}
