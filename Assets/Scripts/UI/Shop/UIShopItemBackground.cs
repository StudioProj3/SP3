using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIShopItemBackground : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<bool> _pointerEvent;

    private void Start()
    {
        if (_pointerEvent == null)
        {
            _pointerEvent = new UnityEvent<bool>();
        }
    }

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
}