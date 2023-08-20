using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool _isFinished = false;
    private string _questID;

    public void Initialize(string questID)
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