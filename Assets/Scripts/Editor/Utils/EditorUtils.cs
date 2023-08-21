using System;

using UnityEditor;
using UnityEngine;

public static class EditorUtils
{
    public static void DrawProperty(SerializedObject serializedObject,
        string name, params GUILayoutOption[] options)
    {
        DrawField(serializedObject, name.BackingField(),
            options);
    }

    public static void DrawPropertyRaw(SerializedObject serializedObject,
        string name, params GUILayoutOption[] options)
    {
        DrawFieldRaw(serializedObject, name.BackingField(),
            options);
    }

    public static void DrawField(SerializedObject serializedObject,
        string name, params GUILayoutOption[] options)
    {
        EditorGUILayout.PropertyField(serializedObject.
            FindProperty(name), options);
    }

    public static void DrawFieldRaw(SerializedObject serializedObject,
        string name, params GUILayoutOption[] options)
    {
        EditorGUILayout.PropertyField(serializedObject.
            FindProperty(name), new GUIContent(""), options);
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

    public static void DrawTextureCenter(Texture2D texture2d,
        float length, Material material, Color backgroundColor)
    {
        Horizontal(() =>
        {
            FlexibleSpace(() =>
            {
                DrawTexture(texture2d, length, material,
                    backgroundColor);
            });
        });
    }

    public static void DrawSpriteCenter(Sprite sprite,
        float length, Material material, Color backgroundColor)
    {
        DrawTextureCenter(sprite.texture, length, material,
            backgroundColor);
    }

    public static void DrawTexture(Texture2D texture2d, float length,
        Material material, Color backgroundColor)
    {
        Rect rect = EditorGUILayout.GetControlRect(
            GUILayout.Width(length), GUILayout.Height(length));

        EditorGUI.DrawRect(rect, backgroundColor);
        EditorGUI.DrawPreviewTexture(rect, texture2d,
            material);
    }

    public static void DrawSprite(Sprite sprite, float length,
        Material material, Color backgroundColor)
    {
        DrawTexture(sprite.texture, length, material,
            backgroundColor);
    }

    public static void DrawSpriteFromSheetCenter(Sprite sprite,
        float length, Material material, Color backgroundColor)
    {
        DrawTextureCenter(sprite.GenTexture(), length, material,
            backgroundColor);
    }

    public static void DrawSpriteFromSheet(Sprite sprite,
        float length, Material material, Color backgroundColor)
    {
        DrawTexture(sprite.GenTexture(), length, material,
            backgroundColor);
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
