using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UIHUDQuestInformation : MonoBehaviour
{
    [HorizontalDivider]
    [Header("Data")]
    [SerializeField]
    private UIHUDQuestDisplay _questDisplayPrefab;

    private IObjectPool<UIHUDQuestDisplay> _questDisplays;
    private Dictionary<string, UIHUDQuestDisplay> _displayMap = new();

    public void UpdateDisplayText(string questID, string stepDescription)
    {
        if (_displayMap.TryGetValue(questID, out var display))
        {
            display.SetDisplayText(stepDescription);
        }
        else
        {
            var newDisplay = _questDisplays.Get();
            _displayMap.Add(questID, newDisplay);
            newDisplay.SetDisplayText(stepDescription);
        }
    }

    public void Clear() 
    {
        foreach (var display in _displayMap.Values)
        {
            _questDisplays.Release(display);
        }
        _displayMap.Clear();
    }

    public void ClearQuest(string questID)
    {
        if (_displayMap.TryGetValue(questID, out var display))
        {
            _questDisplays.Release(display);
            _displayMap.Remove(questID);
        }
    }

    private void Awake()
    {
        _questDisplays = new ObjectPool<UIHUDQuestDisplay>
        (
            () => Instantiate(_questDisplayPrefab, transform),
            (UIHUDQuestDisplay display) => display.gameObject.SetActive(true),
            (UIHUDQuestDisplay display) => display.gameObject.SetActive(false)
        );
    }

    private void Start()
    {
        // Call these bindings at start because it is in an additive scene
    }
}
