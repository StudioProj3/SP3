using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    private Dialogue _dialogue;

    public void StartNewDialogue(DialogueInstance dialogueInstance, 
        Transform talkingTransform)
    {
        _dialogue.transform.parent.position = talkingTransform.position;
        _dialogue.Initialize(dialogueInstance);
        _dialogue.StartDialogue();
    }

    public void NextIndex()
    {
        // _dialogue.
    }

    protected override void OnStart()
    {
        _dialogue = GetComponentInChildren<Dialogue>(); 
    }
}