using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropArea :
    MonoBehaviour, IDropHandler
{
    private UIInventory _uiinventory;

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

        int index1 = dragItem.Parent.GetSiblingIndex();
        int index2 = transform.GetSiblingIndex();

        // Perform the actual inventory item swap
        _uiinventory.MainInventory.Swap(index1, index2);

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
    }

    private void Awake()
    {
        DragItem = transform.GetChild(0).
            GetComponent<UIDragItem>();
        _uiinventory = GameObject.FindWithTag("UIInventory").
            GetComponent<UIInventory>();
    }
}
