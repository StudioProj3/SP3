using UnityEngine;
using UnityEngine.Pool;

public class UIHUDQuestInformation : MonoBehaviour
{
    [HorizontalDivider]
    [Header("Data")]
    [SerializeField]
    private UIHUDQuestDisplay _questDisplayPrefab;

    private IObjectPool<UIHUDQuestDisplay> _questDisplays;

    private UIHUDQuestDisplay _display;

    public void OnQuestStart(string stepDescription)
    {
        _display.SetDisplayText(stepDescription);
    }

    private void Awake()
    {
        _questDisplays = new ObjectPool<UIHUDQuestDisplay>(() => 
            Instantiate(_questDisplayPrefab));

        // For testing
        _display = GetComponentInChildren<UIHUDQuestDisplay>(true);
    }

    private void Start()
    {
        // Call these bindings at start because it is in an additive scene
    }
}
