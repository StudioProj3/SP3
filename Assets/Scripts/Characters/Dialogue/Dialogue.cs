using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _dialogueTitle;

    [SerializeField]
    private TMP_Text _dialogueTextbox;

    private DialogueInstance _dialogueInstance; 
    private int _dialogueIndex; 

    public void Initialize(DialogueInstance dialogue)
    {
        _dialogueInstance = dialogue;
        _dialogueIndex = 0;
        _dialogueTitle.text = dialogue.Data.SpeakerName;
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        GameManager.Instance.ChangeGameState(GameState.Dialogue);
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

    public void SetText(string text)
    {
        _dialogueTextbox.text = text;
    }
}