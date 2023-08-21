using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private DialogueInstance _dialogueInstance; 

    public void Initialize(DialogueInstance dialogue)
    {
        _dialogueInstance = dialogue;
    }

}