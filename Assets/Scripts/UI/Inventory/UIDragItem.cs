using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragItem :
    MonoBehaviour, IDragHandler, IBeginDragHandler,
    IEndDragHandler
{
    public Transform Parent { get; set; }

    private UIInventory _uiinventory;

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        BringToFront();
        _uiinventory.HideHoverPanel();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RevertOrder();
        _uiinventory.ShowHoverPanel();
    }

    private void Awake()
    {
        Parent = transform.parent;
        _uiinventory = GameObject.FindWithTag("UIInventory").
            GetComponent<UIInventory>();
    }

    private void BringToFront()
    {
        transform.SetParent(transform.parent.parent);
        transform.SetAsLastSibling();
    }

    private void RevertOrder()
    {
        transform.SetParent(Parent);
    }
}
