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
}