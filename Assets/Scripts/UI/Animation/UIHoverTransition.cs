using UnityEngine;
using UnityEngine.EventSystems;

using static DebugUtils;

public class UIHoverTransition :
    UITransitionBase, IPointerEnterHandler, IPointerExitHandler
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }

    [SerializeField]
    private Direction _direction = Direction.Up;

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
