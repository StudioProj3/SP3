using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    [field: SerializeField]
    public string DisplayDescription { get; private set;}

    protected string _questID;
    private bool _isFinished = false;

    public virtual void Initialize(string questID)
    {
        _questID = questID;
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
}