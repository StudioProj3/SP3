using UnityEngine;
using UnityEngine.Pool;

public class UIHUDQuestInformation : MonoBehaviour
{
    [HorizontalDivider]
    [Header("Data")]
    [SerializeField]
    private UIHUDQuestDisplay _questDisplayPrefab;

    private IObjectPool<UIHUDQuestDisplay> _questDisplays;

    private void Awake()
    {
        _questDisplays = new ObjectPool<UIHUDQuestDisplay>(() => 
            Instantiate(_questDisplayPrefab));
    }
}
