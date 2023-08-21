using UnityEngine;

using static DebugUtils;

public class TalkNPCQuestStep : QuestStep
{
    [field: SerializeField]
    public DialogueInstance Dialogue { get; private set; }

    private int _dialogueIndex; 

    private void Start()
    {
        Assert(Dialogue != null,
            "Dialogue is null in Quest step. ID:" + _questID);
        
        // Reset the dialogue index to 0.
        _dialogueIndex = 0;
    }

}