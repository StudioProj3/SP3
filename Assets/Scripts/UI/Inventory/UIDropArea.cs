using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropArea : EventTrigger
{
    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData == null)
        {
            return;
        }

        Debug.Log("Ondrop");
        GameObject gameObject = eventData.pointerDrag;
        UIDragItem dragItem = gameObject.
            GetComponent<UIDragItem>();

        gameObject.transform.position =
            transform.position;
        dragItem.Parent = transform;
    }
}
