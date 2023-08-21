using UnityEngine;

using static DebugUtils;

public class TalkNPCQuestStep : QuestStep
{
    [SerializeField]
    private string _NPCtag;

    [field: SerializeField]
    public DialogueInstance Dialogue { get; private set; }

    private GameObject _targetNPCObject;
    private Dialogue _targetDialogue;

    private int _dialogueIndex; 

    private void Start()
    {
        Assert(Dialogue != null || string.IsNullOrEmpty(_NPCtag),
            "Dialogue or tag is null in Quest step. ID:" + _questID);
        
        // Reset the dialogue index to 0.
        _dialogueIndex = 0;

        _targetNPCObject = GameObject.FindWithTag(_NPCtag);
        _dialogue = _targetNPCObject.GetComponent<Dialogue>();
    }

}