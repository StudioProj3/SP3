using System.Collections.Generic;

using UnityEditor;

using static DebugUtils;

public abstract class ReorderEditorBase : Editor
{
    public enum Type
    {
        Field,
        Property,
    }

    private SortedList<uint, SerializedProperty> _properties =
        new();
    private uint _currentOrder = 1;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        foreach (KeyValuePair<uint, SerializedProperty> property
            in _properties)
        {
            EditorGUILayout.PropertyField(property.Value);
        }

        serializedObject.ApplyModifiedProperties();
    }

    protected void Add(Type type, string name, uint order = 0)
    {
        // Convert to the backing field name if necessary
        name = (type == Type.Property) ? name.BackingField() : name;

        SerializedProperty property = serializedObject.
            FindProperty(name);

        Assert(property != null,
            "Field or Property could not be found");

        _properties.Add(order == 0 ? _currentOrder++ : order,
            property);
    }

    protected void AddField(string name, uint order = 0)
    {
        Add(Type.Field, name, order);
    }

    protected void AddProperty(string name, uint order = 0)
    {
        Add(Type.Property, name, order);
    }

    protected virtual void OnEnable()
    {
        _properties = new();
        _currentOrder = 1;
    }
}
