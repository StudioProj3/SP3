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

    private void OnEnable()
    {
        _statsListProperty = serializedObject.
            FindProperty("_statInitializerList");
        _statsReorderableList =
            new(serializedObject, _statsListProperty,
            true, true, true, true)
        {
            drawHeaderCallback = (Rect rect) =>
                EditorGUI.LabelField(rect, "Stats"),

            elementHeightCallback = (int index) =>
                EditorGUI.GetPropertyHeight(_statsListProperty.
                GetArrayElementAtIndex(index)),

            onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
            {
                GenericMenu menu = new();
                menu.AddItem(new GUIContent("Stat"), false, OnAddStatHandler,
                    new StatsCreationParams() { isBounded = false });
                menu.AddItem(new GUIContent("Bounded Stat"), false,
                    OnAddStatHandler, new StatsCreationParams()
                    { isBounded = true });

                menu.ShowAsContext();
            },

            onRemoveCallback = (ReorderableList list) => 
            {
                Stats stats = target as Stats;
                stats.RemoveStatAtIndex(list.index);
            }, 

            drawElementCallback = (Rect rect, int index, bool isActive,
                bool isFocused) => 
            {
                var element = _statsListProperty.
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

        _instancedStatsListProperty = serializedObject.
            FindProperty("_instancedStatInitializerList");
        _instancedStatsReorderableList =
            new(serializedObject, _instancedStatsListProperty,
            true, true, true, true)
        {
            drawHeaderCallback = (Rect rect) =>
                EditorGUI.LabelField(rect, "Instanced Stats"),

            elementHeightCallback = (int index) =>
                EditorGUI.GetPropertyHeight(_instancedStatsListProperty.
                GetArrayElementAtIndex(index)),

            onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
            {
                GenericMenu menu = new();
                menu.AddItem(new GUIContent("Stat"), false,
                    OnAddInstancedStatHandler, new StatsCreationParams()
                    { isBounded = false });
                menu.AddItem(new GUIContent("Bounded Stat"), false,
                    OnAddInstancedStatHandler, new StatsCreationParams()
                    { isBounded = true });

                menu.ShowAsContext();
            },

            onRemoveCallback = (ReorderableList list) => 
            {
                Stats stats = target as Stats;
                stats.RemoveInstancedStatAtIndex(list.index);
            }, 

            drawElementCallback = (Rect rect, int index, bool isActive,
                bool isFocused) => 
            {
                var element = _instancedStatsListProperty.
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