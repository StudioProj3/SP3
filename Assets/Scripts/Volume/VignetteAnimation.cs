using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteAnimation : MonoBehaviour
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
    private float _lerpFactor = 0.05f;

    [SerializeField]
    [Range(50, 100)]
    [Tooltip("Update frequency in number of times a second")]
    private uint _frequency = 60;

    [SerializeField]
    [Range(0f, 1f)]
    private float _minValue = 0.25f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _maxValue = 0.5f;

    private float _accumulator;
    private IModifiableValue _sanityValue;
    private Volume _volume;
    private Vignette _vignette;

    private void LateUpdate()
    {
        // Delta time is used to ensure the lerping speed is
        // independent of the user's frame rate
        _accumulator += Time.deltaTime;

        // Perform 1 or more cycles depending on the time
        // stored in the `_accumulator`
        while (_accumulator >= (1f / _frequency))
        {
            float ratio = _sanityValue.Value / _sanityValue.Max;

            float currentValue = (float)_vignette.intensity;
            float newValue = ((1f - ratio) * (_maxValue - _minValue)) +
                _minValue;

            _vignette.intensity.value =
                Mathf.Lerp(currentValue, newValue, _lerpFactor);

            // Decrease `_accumulator` after 1 iteration
            _accumulator -= 1f / _frequency;
        }
    }

    private void Awake()
    {
        _volume = GetComponent<Volume>();
        _volume.profile.TryGet(out _vignette);

        _sanityValue = _stats.GetStat(_statType);
    }
}
