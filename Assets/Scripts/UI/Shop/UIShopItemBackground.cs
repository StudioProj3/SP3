using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShopItemBackground : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    private UnityEvent<bool> _pointerEvent;

    public void SubscribePointerEvent(UnityAction<bool> action)
    {
        _pointerEvent.AddListener(action);
    }

    public void UnsubscribePointerEvent(UnityAction<bool> action)
    {
        _pointerEvent.RemoveListener(action);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _pointerEvent?.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _pointerEvent?.Invoke(false);
    }
    
    private void Awake()
    {
        if (_pointerEvent == null)
        {
            _pointerEvent = new UnityEvent<bool>();
        }
    }
}
