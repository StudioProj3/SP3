using UnityEngine;
using UnityEngine.UI;

public class UIHUDStatBar : MonoBehaviour
{
    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private StatType _statType;

    private RectMask2D _statBarLeft;
    private RectMask2D _statBarRight;

    private float _length;

    private void Update()
    {
        IModifiableValue value = _stats.GetStat(_statType);
        float ratio = value.Value / value.Max;

        float padding = (1f - ratio) * _length;

        _statBarLeft.padding = new(padding, 0f, 0f, 0f);
        _statBarRight.padding = new(0f, 0f, padding, 0f);
    }

    private void Awake()
    {
        _statBarLeft = transform.GetChild(0).
            GetComponent<RectMask2D>();
        _statBarRight = transform.GetChild(1).
            GetComponent<RectMask2D>();

        _length = _statBarLeft.rectTransform.sizeDelta.x;
    }
}
