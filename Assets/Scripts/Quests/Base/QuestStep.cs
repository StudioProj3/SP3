using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    [field: SerializeField]
    public string DisplayDescription { get; private set;}

    protected string _questID;
    private bool _isFinished = false;
    private int _stepIndex;

    public virtual void Initialize(string questID, int stepIndex)
    {
        _questID = questID;
        _stepIndex = stepIndex;
    }

    protected void FinishQuestStep()
    {
        if (!_isFinished)
        {
            _isFinished = true;
            QuestManager.Instance.AdvanceQuest(_questID);
            Destroy(gameObject);   
        }
    }

    protected void ChangeState(string newState)
    {
        QuestManager.Instance.QuestStepStateChange(_questID, _stepIndex, new QuestStepState(newState));
    }
}
