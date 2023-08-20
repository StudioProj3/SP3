using UnityEngine;
using UnityEngine.Assertions;

using static DebugUtils;

public class Quest
{
    public QuestInfo Info { get; private set; }
    public QuestState state; 
    private int _currentQuestStepIndex;

    public Quest(QuestInfo info)
    {
        Info = info;
        state = QuestState.RequirementsNotMet;
        _currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return _currentQuestStepIndex < Info.QuestSteps.Length;
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject prefab = GetCurrentQuestStepPrefab();
        if (prefab != null)
        {
            Object.Instantiate(prefab, parentTransform);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        Assert(CurrentStepExists(), 
            $"Step index out of range. Quest ID = {Info.name}");

        return Info.QuestSteps[_currentQuestStepIndex];
    }

}