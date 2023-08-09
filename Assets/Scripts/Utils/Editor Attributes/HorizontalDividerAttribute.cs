using UnityEngine;

public class HorizontalDividerAttribute : PropertyAttribute
{
    // Used to determine whether the divider is for
    // a `Header` or for normal fields in the inspector
    public enum Type
    {
        Header,
        Normal,
    }

    public Type Target { get; private set; }
    public float Height { get; private set; }

    public HorizontalDividerAttribute(Type target = Type.Header,
        float height = 2f)
    {
        Target = target;
        Height = height;
    }
}
