using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HorizontalDividerAttribute))]
public class HorizontalDividerDrawer : DecoratorDrawer
{
    private HorizontalDividerAttribute Attr
    {
        get => (HorizontalDividerAttribute)attribute;
    }

    private float Height
    {
        get => Attr.Height;
    }

    private HorizontalDividerAttribute.Type Target
    {
        get => Attr.Target;
    }

    private readonly float _halfLineHeight =
        EditorGUIUtility.singleLineHeight / 2f;

    public override float GetHeight()
    {
        // Leave more space when the target is `Normal`
        float multiplier =
            Target == HorizontalDividerAttribute.Type.Normal ? 2f : 1f;

        return (_halfLineHeight * multiplier) + Height;
    }

    public override void OnGUI(Rect position)
    {
        base.OnGUI(position);

        // Create the horizontal line rectangle
        Rect rect = EditorGUI.IndentedRect(position);
        rect.y += _halfLineHeight;
        rect.height = Height;

        // Draw the actual divider as grey
        EditorGUI.DrawRect(rect, Color.grey);
    }
}
