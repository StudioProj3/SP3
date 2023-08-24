using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfo", menuName = "Scriptable Objects/Quest Info")]
public class QuestInfo : ScriptableObject, INameable
{
    [field: SerializeField]
    public string ID { get; protected set; }

    [field: SerializeField]
    public string Name { get; protected set; }

    [field: HorizontalDivider]
    [field: Header("Prerequisites")]
    [field: SerializeField]

    public QuestInfo[] PrerequisiteQuests { get; protected set; }

    [field: HorizontalDivider]
    [field: SerializeField]
    public GameObject[] QuestSteps { get; protected set; }

    [field: HorizontalDivider]
    [field: Header("Settings")]

    [field: SerializeField]
    public bool Autocomplete { get; protected set; }

    private void OnValidate()
    {
#if UNITY_EDITOR 
        ID = name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}