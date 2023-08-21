using UnityEngine;
using UnityEditor;

using static EditorUtils;

[CustomEditor(typeof(BasicRecipe))]
public class BasicRecipeEditor :
    RecipeEditorBase
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BasicRecipe recipe = (BasicRecipe)target;

        ItemBase[][] items = { new []{ recipe.Row1Col1, recipe.Row1Col2 },
            new []{ recipe.Row2Col1, recipe.Row2Col2 } };
        string[][] itemsName = { new []{ "Row1Col1", "Row1Col2" },
            new []{ "Row2Col1", "Row2Col2" } };

        for (int i = 0; i < items.Length; ++i)
        {
            Horizontal(() =>
            {
                FlexibleSpace(() =>
                {
                    Vertical(() =>
                    {
                        DrawItem(items[i][0]);
                        DrawPropertyRaw(serializedObject, itemsName[i][0],
                            GUILayout.Width(width));
                    });
                });

                FlexibleSpace(() =>
                {
                    Vertical(() =>
                    {
                        DrawItem(items[i][1]);
                        DrawPropertyRaw(serializedObject, itemsName[i][1],
                            GUILayout.Width(width));
                    });
                });
            });
        }

        serializedObject.ApplyModifiedProperties();
    }
}
