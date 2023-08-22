using TMPro;
using UnityEngine;

using static DebugUtils;

public class TalkNPCQuestStep : QuestStep
{
    [SerializeField]
    private string _npcTag;

    [field: SerializeField]
    public DialogueInstance Dialogue { get; private set; }

    private bool _isDialogueComplete = false;

    private GameObject _targetNPCObject;
    private DialoguePoint _targetDialogue;


    private void Start()
    {
        Assert(Dialogue != null || string.IsNullOrEmpty(_npcTag),
            "Dialogue or tag is null in Quest step. ID:" + _questID);
        
        // Reset the dialogue index to 0.
        _isDialogueComplete = true;

        _targetNPCObject = GameObject.FindWithTag(_npcTag);
        _targetDialogue = _targetNPCObject.GetComponent<DialoguePoint>();
        _targetDialogue.InjectData(Dialogue);
    }

}