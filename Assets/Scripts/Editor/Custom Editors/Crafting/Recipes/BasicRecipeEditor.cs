using UnityEditor;

[CustomEditor(typeof(BasicRecipe))]
public class BasicRecipeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();



        serializedObject.ApplyModifiedProperties();
    }
}
