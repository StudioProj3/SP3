using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryItemSlot :
    UIItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    private UIInventory _uiinventory;
    private UIHoverPanel _hoverPanel;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("enet");
        _hoverPanel.ShowPanel();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.DelayExecute(() => _hoverPanel.HidePanel(), 0.2f);
    }

    protected override void Awake()
    {
        base.Awake();

        _uiinventory = GameObject.FindWithTag("UIInventory").
            GetComponent<UIInventory>();
        _hoverPanel = _uiinventory.transform.GetChild(2).
            GetComponent<UIHoverPanel>();
    }
}
