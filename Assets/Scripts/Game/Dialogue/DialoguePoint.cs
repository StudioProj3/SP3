using UnityEngine;

using static DebugUtils;

public class DialoguePoint : MonoBehaviour
{
    private DialogueInstance _dialogueInstance;

    public void InjectData(DialogueInstance dialogueInstance) 
    {
        _dialogueInstance = dialogueInstance;
    }

    public void TriggerDialogue()
    {
        Assert(_dialogueInstance is not null, "Dialogue is null");
        DialogueManager.Instance.StartNewDialogue(_dialogueInstance, transform);
    }
}