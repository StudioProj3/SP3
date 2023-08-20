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

        DrawProperty(serializedObject, "Name");
        DrawProperty(serializedObject, "Target");

        DrawBoxCenter(recipe.Target.Sprite, 100f,
            recipe.Material, new Color(0.169f, 0.169f, 0.169f));

        serializedObject.ApplyModifiedProperties();
    }
}
