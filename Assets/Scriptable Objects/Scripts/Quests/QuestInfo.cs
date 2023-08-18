using UnityEngine;

[CreateAssetMenu(fileName = "QuestInfo", menuName = "Scriptable Objects/Quest Info")]
public class QuestInfo : ScriptableObject, INameable
{
    [field: SerializeField]
    public string ID { get; protected set; }

    [field: SerializeField]
    public string Name { get; protected set; }

    [HorizontalDivider]
    [Header("Prerequisites")]

    [SerializeField]

    private QuestInfo[] _prequisiteQuests;

    [HorizontalDivider]
    [SerializeField]
    private GameObject[] _questSteps;
}