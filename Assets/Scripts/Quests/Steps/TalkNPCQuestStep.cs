using TMPro;
using UnityEngine;

using static DebugUtils;

public class TalkNPCQuestStep : QuestStep
{
    [SerializeField]
    private string _npcTag;

    [field: SerializeField]
    public DialogueInstance Dialogue { get; private set; }

    private GameObject _targetNPCObject;
    private DialoguePoint _targetDialogue;

    // To prevent unbinding of non-existent event on DialogueManager
    private bool _hasBinded = false;

    private void Start()
    {
        Assert(Dialogue != null || string.IsNullOrEmpty(_npcTag),
            "Dialogue or tag is null in Quest step. ID:" + _questID);

        DialogueManager.Instance.OnDialogueEnd += DialogueEndHandler; 
        _hasBinded = true;
        
        _targetNPCObject = GameObject.FindWithTag(_npcTag);

        if (_targetNPCObject != null)
        {
            if (_targetNPCObject.TryGetComponent(out _targetDialogue))
            {
                _targetDialogue.InjectData(Dialogue);
            }
        }
    }

    private void OnDestroy()
    {
        if (_hasBinded)
        {
            DialogueManager.Instance.OnDialogueEnd -= DialogueEndHandler;
            _hasBinded = false;
        }
    }

    private void DialogueEndHandler(DialoguePoint point)
    {
        if (_targetDialogue != null && point == _targetDialogue)
        {
            FinishQuestStep();
        }
    }

}