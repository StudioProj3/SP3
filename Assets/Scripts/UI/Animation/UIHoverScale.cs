using UnityEngine;
using UnityEngine.EventSystems;

using static DebugUtils;

public class UIHoverScale :
    UITransitionBase, IPointerEnterHandler, IPointerExitHandler
{
    public enum Axis
    {
        X,
        Y,
        XY,
    }

    [SerializeField]
    private Axis _axis = Axis.XY;

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
        Vector2 scale = rectTransform.localScale;

        float delta = (Time.deltaTime / _duration) * _magnitude;

        switch (_axis)
        {
            case Axis.X:
                scale.x = StepX(scale.x, delta);
                break;

            case Axis.Y:
                scale.y = StepY(scale.y, delta);
                break;

            case Axis.XY:
                Pair<float, float> result =
                    StepXY(scale.x, scale.y, delta);
                scale.x = result.First;
                scale.y = result.Second;
                break;

            default:
                Fatal("Unhandled axis type");
                break;
        }

        rectTransform.localScale = new(scale.x, scale.y);
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();

        if (_moveSelf)
        {
            _original = _rectTransform.localScale;

            return;
        }

        _original = _otherRectTransform.localScale;
    }

    private float StepX(float x, float delta)
    {
        x += _hover ? delta : -delta;
        float minX = Mathf.Min(_original.x + _magnitude,
            _original.x);
        float maxX = Mathf.Max(_original.x + _magnitude,
            _original.x);

        return Mathf.Clamp(x, minX, maxX);
    }

    private float StepY(float y, float delta)
    {
        y += _hover ? delta : -delta;
        float minY = Mathf.Min(_original.y + _magnitude,
            _original.y);
        float maxY = Mathf.Max(_original.y + _magnitude,
            _original.y);

        return Mathf.Clamp(y, minY, maxY);
    }

    private Pair<float, float> StepXY(float x,
        float y, float delta)
    {
        Pair<float, float> result = new(x, y);
        result.First = StepX(result.First, delta);
        result.Second = StepY(result.Second, delta);

        return result;
    }
}
