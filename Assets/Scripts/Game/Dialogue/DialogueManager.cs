using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : Singleton<DialogueManager>
{
    private Dialogue _dialogue;

    public void StartNewDialogue(DialogueInstance dialogueInstance, 
        Transform talkingTransform)
    {
        _dialogue.transform.parent.position = talkingTransform.position;
        _dialogue.Initialize(dialogueInstance);
    }

    protected override void OnAwake()
    {
        _dialogue = GetComponentInChildren<Dialogue>(); 
    }
}