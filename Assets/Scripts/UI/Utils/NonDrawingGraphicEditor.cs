#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

// Don't forget to put this file inside a 'Editor' folder
[CanEditMultipleObjects, CustomEditor(typeof(NonDrawingGraphic), false)]
public class NonDrawingGraphicEditor : GraphicEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(m_Script, new GUILayoutOption[0]);
        EditorGUI.EndDisabledGroup();

        // Skipping AppearanceControlsGUI
        RaycastControlsGUI();
        serializedObject.ApplyModifiedProperties();
    }
}

#endif
