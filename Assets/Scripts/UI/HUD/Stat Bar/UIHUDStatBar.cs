using UnityEngine;
using UnityEngine.UI;

public class UIHUDStatBar : MonoBehaviour
{
    [HorizontalDivider]
    [Header("Stat Parameters")]

    [SerializeField]
    private Stats _stats;

    [SerializeField]
    private StatType _statType;

    [HorizontalDivider]
    [Header("Transition Parameters")]

    [SerializeField]
    [Range(0.01f, 1f)]
    [Tooltip("The amount of lerp to use with `1f` being no lerp " +
        "and `0.01f` being slowest lerp")]
    private float _lerpFactor = 0.2f;

    [SerializeField]
    [Range(50, 100)]
    [Tooltip("Update frequency in number of times a second")]
    private uint _frequency = 60;

    private RectMask2D _statBarLeft;
    private RectMask2D _statBarRight;
    private float _length;
    private IModifiableValue _value;
    private float _accumulator;

    // Use `LateUpdate` as the stats might change during `Update`
    private void LateUpdate()
    {
        // Delta time is used to ensure the lerping speed is
        // independent of the user's frame rate
        _accumulator += Time.deltaTime;

        // Perform 1 or more cycles depending on the time
        // stored in the `_accumulator`
        while (_accumulator >= (1f / _frequency))
        {
            float ratio = _stats.GetStat(_statType).Value / _stats.GetStat(_statType).Max;

            float padding = _statBarLeft.padding.x;
            float newPadding = (1f - ratio) * _length;

            SetBothPadding(Mathf.Lerp(padding, newPadding, _lerpFactor));

            // Decrease `_accumulator` after 1 iteration
            _accumulator -= 1f / _frequency;
        }
    }

    private void Awake()
    {
        _statBarLeft = transform.GetChild(4).
            GetComponent<RectMask2D>();
        _statBarRight = transform.GetChild(5).
            GetComponent<RectMask2D>();

        _length = _statBarLeft.rectTransform.sizeDelta.x;
        _value = _stats.GetStat(_statType);
    }

    private void SetLeftPadding(float newLeftPadding)
    {
        _statBarLeft.padding = new(newLeftPadding, 0f, 0f, 0f);
    }

    private void SetRightPadding(float newRightPadding)
    {
        _statBarRight.padding = new(0f, 0f, newRightPadding, 0f);
    }

    private void SetBothPadding(float newBothPadding)
    {
        SetLeftPadding(newBothPadding);
        SetRightPadding(newBothPadding);
    }
}
