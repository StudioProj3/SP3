using System;

[Serializable]
public class QuestData
{
    public QuestState state;
    public int stepIndex;
    public QuestStepState[] stepStates;

    public QuestData(QuestState state, int stepIndex, QuestStepState[] stepStates)
    {
        this.state = state;
        this.stepIndex = stepIndex;
        this.stepStates = stepStates;
    }
}