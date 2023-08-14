using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(Stats))]
public class CustomStatsEditor : Editor
{
    private struct StatsCreationParams
    {
        public bool isBounded;
    }

    private SerializedProperty _statsListProperty;
    private ReorderableList _statsReorderableList;
    private void OnEnable()
    {
        _statsListProperty = serializedObject.FindProperty("_statInitializerList");
        _statsReorderableList = new ReorderableList(serializedObject, _statsListProperty,
            true, true, true, true)
        {
            drawHeaderCallback =
            (Rect rect) => EditorGUI.LabelField(rect, "Stats"),

            drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = _statsListProperty.GetArrayElementAtIndex(index);
                // EditorGUI.PropertyField(rect, element);
                rect.x += 10;
                rect.width -= 10;
                ReorderableList.defaultBehaviours.DrawElement(rect, element, element.serializedObject, isActive,
                isFocused, true, true);

            },

            onRemoveCallback =
            (ReorderableList list) => 
            {
                Stats stats = target as Stats;
                stats.RemoveStatAtIndex(list.index);
            },

            onAddDropdownCallback =
            (Rect buttonRect, ReorderableList list) =>
            {
                GenericMenu menu = new();
                menu.AddItem(new GUIContent("Stat"), false, OnAddStatHandler,
                    new StatsCreationParams() { isBounded = false });
                menu.AddItem(new GUIContent("Bounded Stat"), false, OnAddStatHandler,
                    new StatsCreationParams() { isBounded = true });

                menu.ShowAsContext();
            },

            drawElementBackgroundCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, true);
            },

            elementHeight = 19,
            elementHeightCallback = (int index) =>
                EditorGUI.GetPropertyHeight(_statsListProperty.GetArrayElementAtIndex(index))
        };
    }


    private void OnAddStatHandler(object clicked)
    {
        var creationParameters = (StatsCreationParams)clicked;

        Stats stats = target as Stats;
        if (creationParameters.isBounded)
        {
            stats.CreateBoundedStat();
        }
        else
        {
            stats.CreateStat();
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        _statsReorderableList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}