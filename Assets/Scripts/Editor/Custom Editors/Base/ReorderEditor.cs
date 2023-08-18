using log4net.Filter;
using System.Collections.Generic;

using UnityEditor;

using static DebugUtils;

public abstract class ReorderEditor : Editor
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
        if (type == Type.Property)
        {
            name = "<" + name + ">k__BackingField";
        }

        SerializedProperty property = serializedObject.
            FindProperty(name);

        Assert(property != null,
            "Field or Property could not be found");

        _properties.Add(order == 0 ? _currentOrder++ : order,
            property);
        UnityEngine.Debug.Log(_currentOrder); 
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
