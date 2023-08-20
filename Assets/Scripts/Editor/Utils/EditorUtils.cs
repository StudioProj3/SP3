using UnityEditor;

public static class EditorUtils
{
    public static void DrawProperty(SerializedObject serializedObject,
        string name)
    {
        EditorGUILayout.PropertyField(serializedObject.
            FindProperty(name.BackingField()));
    }
}
