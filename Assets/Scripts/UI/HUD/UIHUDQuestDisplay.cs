using UnityEngine;
using TMPro;

public class UIHUDQuestDisplay : MonoBehaviour
{
    private TMP_Text _displayText;

    private void Awake()
    {
        _displayText = GetComponentInChildren<TMP_Text>(true);
    }

    public void SetDisplayText(string text)
    {
        _displayText.text = text;
    }
}
