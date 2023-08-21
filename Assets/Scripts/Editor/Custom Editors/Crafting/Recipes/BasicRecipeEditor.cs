using UnityEngine;
using UnityEditor;

using static EditorUtils;

[CustomEditor(typeof(BasicRecipe))]
public class BasicRecipeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BasicRecipe recipe = (BasicRecipe)target;
        ItemBase itemTarget = recipe.Target;

        float width = 100f;

        DrawProperty(serializedObject, "Name");
        DrawProperty(serializedObject, "TargetQuantity");

        Color backgroundColor = new(0.169f, 0.169f, 0.169f);
        Material material = recipe.Material;

        DrawSprite(itemTarget);

        Horizontal(() =>
        {
            FlexibleSpace(() =>
            {
                DrawPropertyRaw(serializedObject, "Target",
                    GUILayout.Width(width));
            });
        });

        serializedObject.ApplyModifiedProperties();

        void DrawSprite(ItemBase item)
        {
            if (item)
            {
                if (itemTarget.Atlas)
                {
                    DrawSpriteFromSheetCenter(recipe.Target.Sprite,
                        width, material, backgroundColor);
                }
                else
                {
                    DrawSpriteCenter(recipe.Target.Sprite, width,
                        material, backgroundColor);
                }
            }
        }
    }
}
