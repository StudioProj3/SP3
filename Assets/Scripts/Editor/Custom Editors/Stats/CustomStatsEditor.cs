using System;

using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(Stats))]
public class CustomStatsEditor : Editor
{
    private struct StatsCreationParams
    {
        public bool isBounded;
    }

    private SerializedProperty _statsListProperty;
    private ReorderableList _statsReorderableList;

    private SerializedProperty _instancedStatsListProperty;
    private ReorderableList _instancedStatsReorderableList;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        _statsReorderableList.DoLayoutList();
        EditorGUILayout.Space();

        // Horizontal divider
        Rect rect = EditorGUILayout.GetControlRect(false, 1f);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1f));

        EditorGUILayout.Space();

        _instancedStatsReorderableList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private void ListDrawElementBackgroundCallback(Rect rect,
        int index, bool isActive, bool isFocused)
    {
        ReorderableList.defaultBehaviours.
            DrawElementBackground(rect, index, isActive, isFocused, true);
    }

    private void Generate(string propertyName, string labelName,
        ref SerializedProperty property, ref ReorderableList list,
        GenericMenu.MenuFunction2 addHandler,
        Action<Stats, int> removeHandler)
    {
        property = serializedObject.
            FindProperty(propertyName);

        var localProperty = property;

        list =
            new(serializedObject, property,
            true, true, true, true)
        {
            drawHeaderCallback = (Rect rect) =>
                EditorGUI.LabelField(rect, labelName),

            elementHeightCallback = (int index) =>
                EditorGUI.GetPropertyHeight(localProperty.
                GetArrayElementAtIndex(index)),

            onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
            {
                GenericMenu menu = new();
                menu.AddItem(new GUIContent("Stat"), false, addHandler,
                    new StatsCreationParams() { isBounded = false });
                menu.AddItem(new GUIContent("Bounded Stat"), false,
                    addHandler, new StatsCreationParams()
                    { isBounded = true });

                menu.ShowAsContext();
            },

            onRemoveCallback = (ReorderableList list) => 
            {
                Stats stats = target as Stats;
                removeHandler(stats, list.index);
            }, 

            drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) => 
            {
                var element = localProperty.
                    GetArrayElementAtIndex(index);

                rect.x += 10;
                rect.width -= 10;

                ReorderableList.defaultBehaviours.
                    DrawElement(rect, element, element.serializedObject,
                    isActive, isFocused, true, true);
            },

            drawElementBackgroundCallback = ListDrawElementBackgroundCallback,

            elementHeight = 19,
        };
    }

    private void OnEnable()
    {
        Generate("_statInitializerList", "Stats", ref _statsListProperty,
            ref _statsReorderableList, OnAddStatHandler,
            (stats, index) => { stats.RemoveStatAtIndex(index); });
        Generate("_instancedStatInitializerList", "Instanced Stats",
            ref _instancedStatsListProperty, ref _instancedStatsReorderableList,
            OnAddInstancedStatHandler,
            (stats, index) => { stats.RemoveInstancedStatAtIndex(index); });
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

    private void OnAddInstancedStatHandler(object clicked)
    {
        var creationParameters = (StatsCreationParams)clicked;

        Stats stats = target as Stats;
        if (creationParameters.isBounded)
        {
            stats.CreateBoundedInstancedStat();
        }
        else
        {
            stats.CreateInstancedStat();
        }
    }
}
