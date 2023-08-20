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

        TextCenter("Target", FontStyle.Bold);

        serializedObject.ApplyModifiedProperties();
    }
}
