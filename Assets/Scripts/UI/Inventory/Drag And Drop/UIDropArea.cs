using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropArea :
    MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null)
        {
            return;
        }

        GameObject gameObject = eventData.pointerDrag;
        UIDragItem dragItem = gameObject.
            GetComponent<UIDragItem>();

        gameObject.transform.position =
            transform.position;
        dragItem.Parent = transform;
    }
}
