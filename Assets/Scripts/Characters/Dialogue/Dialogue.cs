using UnityEngine;
using TMPro;

using static DebugUtils;

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
        Assert(dialogue.Data.Dialogue.Count > 0, "There is no dialogue data.");

        _dialogueInstance = dialogue;
        _dialogueIndex = 0;
        _dialogueTitle.text = dialogue.Data.SpeakerName;
    }

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        GameManager.Instance.CurrentState = GameState.Dialogue;
        UIManager.Instance.ShowBars(true);
        UIManager.Instance.ShowHUD(false);
        SetText(_dialogueInstance.Data.Dialogue[0]);
    }

    public void EndDialogue()
    {
        gameObject.SetActive(false);
        // NOTE (Chris): I reckon this could cause some issues.
        GameManager.Instance.CurrentState = GameState.Play;
        UIManager.Instance.ShowBars(false);
        UIManager.Instance.ShowHUD(true);
    }

    public bool NextText(out string text)
    {
        text = null;

        if (_dialogueIndex < _dialogueInstance.Data.Dialogue.Count - 1)
        {
            _dialogueIndex++;
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