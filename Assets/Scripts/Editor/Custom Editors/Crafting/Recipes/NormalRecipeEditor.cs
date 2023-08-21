using UnityEngine;
using UnityEditor;

using static EditorUtils;

[CustomEditor(typeof(NormalRecipe))]
public class NormalRecipeEditor :
    RecipeEditorBase
{
    public override void OnInspectorGUI()
    {
        width = 85f;

        base.OnInspectorGUI();

        NormalRecipe recipe = (NormalRecipe)target;

        ItemBase[][] items = {
            new []{ recipe.Row1Col1, recipe.Row1Col2, recipe.Row1Col3 },
            new []{ recipe.Row2Col1, recipe.Row2Col2, recipe.Row2Col3 },
            new []{ recipe.Row3Col1, recipe.Row3Col2, recipe.Row3Col3 }
        };
        string[][] itemsName = {
            new []{ "Row1Col1", "Row1Col2", "Row1Col3" },
            new []{ "Row2Col1", "Row2Col2", "Row2Col3" },
            new []{ "Row3Col1", "Row3Col2", "Row3Col3" }
        };

        for (int i = 0; i < items.Length; ++i)
        {
            Horizontal(() =>
            {
                for (int j = 0; j < items[i].Length; ++j)
                {
                    FlexibleSpace(() =>
                    {
                        Vertical(() =>
                        {
                            DrawItem(items[i][j]);
                            DrawPropertyRaw(serializedObject,
                                itemsName[i][j], GUILayout.Width(width));
                        });
                    });
                }
            });
        }

        serializedObject.ApplyModifiedProperties();
    }
}
