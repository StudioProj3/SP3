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
        // TODO (Chris): Might need to make an interface to store 'triggers'
        // of dialogues in the dialogue manager.
        if (_dialogueInstance is not null && 
            DialogueManager.Instance.CanStartDialogue)
        {
            DialogueManager.Instance.StartNewDialogue(this,
                _dialogueInstance, transform);
        }
        
    }
}