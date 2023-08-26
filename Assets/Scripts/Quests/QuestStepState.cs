using System;

[Serializable]
public class QuestStepState
{
    public string state;

    public QuestStepState(string state)
    {
        this.state = state;
    }

    public QuestStepState()
    {
        state = string.Empty;
    }
}