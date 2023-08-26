using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropArea :
    MonoBehaviour, IDropHandler
{
    public UIDragItem DragItem { get; set; }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null)
        {
            return;
        }

        GameObject gameObject = eventData.pointerDrag;
        UIDragItem dragItem = gameObject.
            GetComponent<UIDragItem>();

        if (!dragItem)
        {
            return;
        }

        dragItem.DropArea.DragItem = DragItem;

        // Perform the switch of parents
        DragItem.Parent = dragItem.Parent;
        DragItem.RevertParent();
        dragItem.Parent = transform;
        dragItem.RevertParent();

        DragItem.GetComponent<RectTransform>().
            anchoredPosition = new(0f, 4.9f);
        dragItem.GetComponent<RectTransform>().
            anchoredPosition = new(0f, 4.9f);

        DragItem = dragItem;

        Debug.Log("srop");
    }

    private void Awake()
    {
        DragItem = transform.GetChild(0).
            GetComponent<UIDragItem>();
    }
}
