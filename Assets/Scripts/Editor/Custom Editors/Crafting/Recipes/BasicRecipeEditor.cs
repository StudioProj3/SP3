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

        DrawProperty(serializedObject, "Name");
        DrawProperty(serializedObject, "Target");
        DrawProperty(serializedObject, "TargetQuantity");

        if (itemTarget)
        {
            Color backgroundColor = new(0.169f, 0.169f, 0.169f);
            Material material = recipe.Material;

            if (itemTarget.Atlas)
            {
                DrawSpriteFromSheetCenter(recipe.Target.Sprite,
                    100f, material, backgroundColor);
            }
            else
            {
                DrawSpriteCenter(recipe.Target.Sprite, 100f,
                    material, backgroundColor);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
