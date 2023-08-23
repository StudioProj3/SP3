using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryItemSlot :
    UIItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    private UIInventory _uiinventory;
    private UIHoverPanel _hoverPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    private void Awake()
    {
        _uiinventory = GameObject.FindWithTag("UIInventory").
            GetComponent<UIInventory>();
        _hoverPanel = _uiinventory.transform.GetChild(2).
            GetComponent<UIHoverPanel>();
    }
}
