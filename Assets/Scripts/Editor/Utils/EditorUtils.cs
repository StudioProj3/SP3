using System;

using UnityEditor;
using UnityEngine;

public static class EditorUtils
{
    public static void DrawProperty(SerializedObject serializedObject,
        string name)
    {
        DrawField(serializedObject, name.BackingField());
    }

    public static void DrawField(SerializedObject serializedObject,
        string name)
    {
        EditorGUILayout.PropertyField(serializedObject.
            FindProperty(name));
    }

    public static void Label(string text,
        FontStyle fontStyle = FontStyle.Normal)
    {
        GUIStyle style = new(EditorStyles.label);
        style.fontStyle = fontStyle;

        GUILayout.Label(text, style);
    }

    public static void TextCenter(string text,
        FontStyle fontStyle = FontStyle.Normal)
    {
        Horizontal(() =>
        {
            FlexibleSpace(() =>
            {
                Label(text, fontStyle);
            });
        });
    }

    public static void DrawBox(Sprite sprite, float length)
    {
        GUILayout.Box(sprite.texture, GUILayout.Width(length),
            GUILayout.Height(length), GUILayout.ExpandHeight(true));
    }

    public static void DrawBoxCenter(Sprite sprite, float length,
        Material material, Color background)
    {
        Horizontal(() =>
        {
            FlexibleSpace(() =>
            {
                Rect rect = EditorGUILayout.GetControlRect(
                    GUILayout.Width(length), GUILayout.Height(length));

                EditorGUI.DrawRect(rect, background);
                EditorGUI.DrawPreviewTexture(rect, sprite.texture,
                    material);
            });
        });
    }

    public static void Horizontal(Action action)
    {
        EditorGUILayout.BeginHorizontal();

        action();

        EditorGUILayout.EndHorizontal();
    }

    public static void Vertical(Action action)
    {
        EditorGUILayout.BeginVertical();

        action();

        EditorGUILayout.EndVertical();
    }

    public static void FlexibleSpace(Action action)
    {
        GUILayout.FlexibleSpace();

        action();

        GUILayout.FlexibleSpace();
    }
}
