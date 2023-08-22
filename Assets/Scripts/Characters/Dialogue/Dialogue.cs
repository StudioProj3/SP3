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

}