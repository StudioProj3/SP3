using UnityEngine;
using UnityEditor;

using static DebugUtils;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    private ShowIfAttribute showIfAttribute;
    private SerializedProperty comparedField;

    public override float GetPropertyHeight(SerializedProperty property,
        GUIContent label)
    {
        if (!CheckShow(property) && showIfAttribute.DisableType ==
            ShowIfAttribute.Type.Hide)
        {
            return 0f;
        }

        return base.GetPropertyHeight(property, label);
    }

    private bool CheckShow(SerializedProperty property)
    {
        showIfAttribute = attribute as ShowIfAttribute;

        string name = showIfAttribute.Name;
        object value = showIfAttribute.Value;

        comparedField = property.serializedObject.FindProperty(name);
        Assert(comparedField != null, "Cannot find property name");

        switch (comparedField.type)
        {
            case "bool":
                return comparedField.boolValue.Equals(value);

            case "Enum":
                return comparedField.enumValueIndex.Equals((int)value);

            default:
                Fatal("Unhandled type");
                return true;
        }
    }

    public override void OnGUI(Rect position, SerializedProperty property,
        GUIContent label)
    {
        if (CheckShow(property))
        {
            EditorGUI.PropertyField(position, property, label);
        }
        else if (showIfAttribute.DisableType ==
            ShowIfAttribute.Type.Hide)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property);
            GUI.enabled = true;
        }
    }
}
