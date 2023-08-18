using UnityEditor;

[CustomEditor(typeof(BasicInventory))]
public class BasicInventoryEditor : Editor
{
    private SerializedProperty _itemInitializerList;
    private SerializedProperty _maxNumSlots;
    private SerializedProperty _maxPerSlot;

    private void OnEnable()
    {
        _itemInitializerList = serializedObject.
            FindProperty("_itemInitializerList");
        _maxNumSlots = serializedObject.
            FindProperty("<MaxNumSlots>k__BackingField");
        _maxPerSlot = serializedObject.
            FindProperty("<MaxPerSlot>k__BackingField");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_maxNumSlots);
        EditorGUILayout.PropertyField(_maxPerSlot);
        EditorGUILayout.PropertyField(_itemInitializerList);
        serializedObject.ApplyModifiedProperties();
    }
}
