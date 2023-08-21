using System;

using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ShowIfAttribute : PropertyAttribute
{
    public enum Type
    {
        Hide,
        ReadOnly
    }

    public string Name { get; private set; }

    public object Value { get; private set; }

    public Type DisableType { get; private set; }

    public ShowIfAttribute(string name, object value,
        bool property = false, Type disableType = Type.Hide)
    {
        Name = property ? name.BackingField() : name;

        Value = value;
        DisableType = disableType;
    }
}
