using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIShopItemBackground : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent<bool> pointerEvent;

    private void Start()
    {
        if (pointerEvent == null)
        {
            pointerEvent = new UnityEvent<bool>();
        }
    }

    public void SubscribePointerEvent(UnityAction<bool> action)
    {
        pointerEvent.AddListener(action);
    }

    public void UnsubscribePointerEvent(UnityAction<bool> action)
    {
        pointerEvent.RemoveListener(action);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerEvent?.Invoke(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerEvent?.Invoke(false);
    }
}
