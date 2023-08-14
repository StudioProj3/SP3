using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Stats))]
public class CustomStatsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Test"))
        {

        }
    }
}