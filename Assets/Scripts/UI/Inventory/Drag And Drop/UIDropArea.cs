using UnityEngine;
using UnityEngine.EventSystems;

public class UIDropArea :
    MonoBehaviour, IDropHandler
{
    public UIDragItem DragItem { get; set; }

    private UIInventoryItemSlot _inventoryItemSlot;
    private InventoryBase _inventory;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData == null)
        {
            return;
        }

        // All info regarding the item currently
        // being dragged (under the user's cursor)
        GameObject gameObject = eventData.pointerDrag;
        UIDragItem dragItem = gameObject.
            GetComponent<UIDragItem>();

        if (!dragItem)
        {
            return;
        }

        UIInventoryItemSlot inventoryItemSlot =
            dragItem.Parent.GetComponent<UIInventoryItemSlot>();
        InventoryBase inventory = inventoryItemSlot.Inventory;

        int index1 = dragItem.Parent.GetSiblingIndex();
        int index2 = transform.GetSiblingIndex();

        // Perform the actual inventory item swap
        inventory.Swap(_inventory, index1, index2);

        if (dragItem.DropArea)
        {
            dragItem.DropArea.DragItem = DragItem;
        }

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
        _inventoryItemSlot =
            GetComponent<UIInventoryItemSlot>();
        _inventory = _inventoryItemSlot.Inventory;
    }
}
