using UnityEngine;

using static DebugUtils;

public class DialogueManager : Singleton<DialogueManager>
{
    private Dialogue _dialogue;
    private DialoguePoint _currentPoint = null;

    public bool CanStartDialogue => _currentPoint == null;

    public void StartNewDialogue(DialoguePoint point, 
        DialogueInstance dialogueInstance, Transform talkingTransform)
    {
        Assert(CanStartDialogue, "Dialogue attempted to start.");
        _currentPoint = point;
        transform.position = talkingTransform.position
            + dialogueInstance.Data.DialogueBoxOffset;
        _dialogue.Initialize(dialogueInstance);
        _dialogue.StartDialogue();
    }

    public void Iterate()
    {
        if (_dialogue.NextText(out var text))
        {
            _dialogue.SetText(text);
        }
    }

    protected override void OnStart()
    {
        _dialogue = GetComponentInChildren<Dialogue>(true); 
    }
}