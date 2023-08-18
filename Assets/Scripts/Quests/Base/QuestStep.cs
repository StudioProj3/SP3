using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool _isFinished = false;

    protected void FinishQuestStep()
    {
        _isFinished = true;
        Destroy(this.gameObject);   
    }
}