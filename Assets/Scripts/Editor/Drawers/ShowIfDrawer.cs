using UnityEngine;
using UnityEditor;

using static DebugUtils;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    private ShowIfAttribute _showIfAttribute;
    private SerializedProperty _comparedField;

    public override float GetPropertyHeight(SerializedProperty property,
        GUIContent label)
    {
        if (!CheckShow(property) && _showIfAttribute.DisableType ==
            ShowIfAttribute.Type.Hide)
        {
            return 0f;
        }

        return base.GetPropertyHeight(property, label);
    }

    private bool CheckShow(SerializedProperty property)
    {
        _showIfAttribute = attribute as ShowIfAttribute;

        string name = _showIfAttribute.Name;
        object value = _showIfAttribute.Value;

        _comparedField = property.serializedObject.FindProperty(name);
        Assert(_comparedField != null, "Cannot find property name");

        switch (_comparedField.type)
        {
            case "bool":
                return _comparedField.boolValue.Equals(value);

            case "Enum":
                return _comparedField.enumValueIndex.Equals((int)value);

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
        else if (_showIfAttribute.DisableType ==
            ShowIfAttribute.Type.Hide)
        {
            // HACK (Cheng Jun): To remove the circle of the object
            // picker since that does not seem to go away even with
            // a height of 0
            position.y = 10000f;

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property);
            GUI.enabled = true;
        }
    }
}
