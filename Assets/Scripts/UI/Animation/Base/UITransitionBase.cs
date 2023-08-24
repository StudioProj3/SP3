using UnityEngine;

public abstract class UITransitionBase :
    MonoBehaviour
{
    [HorizontalDivider]
    [Header("Target")]

    [SerializeField]
    [Tooltip("Whether the transition is applied to this gameobject")]
    protected bool _moveSelf = true;

    [SerializeField]
    [ShowIf("_moveSelf", false)]
    protected RectTransform _otherRectTransform;

    [HorizontalDivider]
    [Header("Transition Parameters")]

    [SerializeField]
    [Range(-10f, 10f)]
    protected float _magnitude = 0.2f;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Time in seconds from the start to the end")]
    protected float _duration = 0.2f;
}
