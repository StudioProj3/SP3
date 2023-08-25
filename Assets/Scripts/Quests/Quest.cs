using UnityEngine;
using UnityEngine.Assertions;

using static DebugUtils;

public class Quest
{
    public QuestInfo Info { get; private set; }
    public QuestState state; 
    private int _currentQuestStepIndex;
    private QuestStepState[] _stepStates;

    public Quest(QuestInfo info)
    {
        Info = info;
        state = QuestState.RequirementsNotMet;
        _currentQuestStepIndex = 0;
        _stepStates = new QuestStepState[Info.QuestSteps.Length];

        for (int i = 0; i < _stepStates.Length; ++i)
        {
            _stepStates[i] = new QuestStepState();
        }

    }

    public void MoveToNextStep()
    {
        _currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return _currentQuestStepIndex < Info.QuestSteps.Length;
    }

    public QuestStep InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject prefab = GetCurrentQuestStepPrefab();
        if (prefab != null)
        {
            QuestStep questStep = Object.Instantiate(prefab, parentTransform)
                .GetComponent<QuestStep>();
            questStep.Initialize(Info.ID, _currentQuestStepIndex);
            return questStep;
        }
        return null;
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        Assert(CurrentStepExists(), 
            $"Step index out of range. Quest ID = {Info.name}");

        return Info.QuestSteps[_currentQuestStepIndex];
    }

    public void StoreQuestStepState(QuestStepState stepState, int index)
    {
        if (index < _stepStates.Length)
        {
            _stepStates[index].state = stepState.state;
        }
        else
        {
            Debug.Log("Could not store quest step state");
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(state, _currentQuestStepIndex, _stepStates);
    }
}