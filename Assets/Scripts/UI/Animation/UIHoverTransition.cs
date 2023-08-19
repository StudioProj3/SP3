using UnityEngine;
using UnityEngine.EventSystems;

using static DebugUtils;

public class UIHoverTransition :
    MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }

    [HorizontalDivider]
    [Header("Target")]

    [SerializeField]
    [Tooltip("Whether the transition is applied to this gameobject")]
    private bool _moveSelf = true;

    [SerializeField]
    [ShowIf("_moveSelf", false)]
    private RectTransform _otherRectTransform;

    [HorizontalDivider]
    [Header("Transition Parameters")]

    [SerializeField]
    private Direction _direction = Direction.Up;

    [SerializeField]
    [Range(0f, 50f)]
    private float _magnitude = 30f;

    [SerializeField]
    [Range(0f, 1f)]
    [Tooltip("Time in seconds from the start to the end")]
    private float _duration = 0.5f;

    private RectTransform _rectTransform;
    private Vector2 _original;
    private bool _hover = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hover = false;
    }

    private void Update()
    {
        RectTransform rectTransform =
            _moveSelf ? _rectTransform : _otherRectTransform;
        Vector2 position = rectTransform.anchoredPosition;

        float delta = (Time.deltaTime / _duration) * _magnitude;

        switch (_direction)
        {
            case Direction.Left:
                position.x += _hover ? -delta : delta;
                position.x = Mathf.Clamp(
                    position.x, _original.x - _magnitude, _original.x);

                break;

            case Direction.Right:
                position.x += _hover ? delta : -delta;
                position.x = Mathf.Clamp(
                    position.x, _original.x, _original.x + _magnitude);

                break;

            case Direction.Up:
                position.y += _hover ? delta : -delta;
                position.y = Mathf.Clamp(
                    position.y, _original.y, _original.y + _magnitude);

                break;

            case Direction.Down:
                position.y += _hover ? -delta : delta;
                position.y = Mathf.Clamp(
                    position.y, _original.y - _magnitude, _original.y);

                break;

            default:
                Fatal("Unhandled direction");
                break;
        }

        rectTransform.anchoredPosition = position;
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (_moveSelf)
        {
            _original = _rectTransform.anchoredPosition;

            return;
        }

        _original = _otherRectTransform.anchoredPosition;
    }
}
