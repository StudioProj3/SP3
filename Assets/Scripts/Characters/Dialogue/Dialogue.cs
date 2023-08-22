using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private DialogueInstance _dialogueInstance; 
    private int _dialogueIndex; 
    public void Initialize(DialogueInstance dialogue)
    {
        _dialogueInstance = dialogue;
        _dialogueIndex = 0;
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
    }

    public bool NextText(out string text)
    {
        text = null;

        if (_dialogueIndex <= _dialogueInstance.Data.Dialogue.Count - 1)
        {
            text = _dialogueInstance.Data.Dialogue[_dialogueIndex];
            return true;
        }
        else
        {
            return false;
        }
    }
}