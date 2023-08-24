using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryItemSlot :
    UIItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private InventoryBase _inventory;

    private UIInventory _uiinventory;
    private UIHoverPanel _hoverPanel;

    private bool _hover = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hoverPanel.ShowPanel();
        UpdateHoverPanel();

        _hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.DelayExecute(() =>
            _hoverPanel.HidePanel(), 0.2f);
        _hover = false;
    }

    private void Update()
    {
        if (_hover)
        {
            _hoverPanel.ShowPanel();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _uiinventory = GameObject.FindWithTag("UIInventory").
            GetComponent<UIInventory>();
        _hoverPanel = _uiinventory.transform.GetChild(2).
            GetComponent<UIHoverPanel>();
    }

    private void UpdateHoverPanel()
    {
        ItemBase item = _inventory.GetItem(
            transform.GetSiblingIndex());

        Pair<string, string> result = new();
        result.First = item != null ? item.Name : "Empty";
        result.Second = item != null ? item.Description : "-";

        _hoverPanel.SetItemName(result.First);
        _hoverPanel.SetItemDescription(result.Second);

        if (item)
        {
            _hoverPanel.ShowAction1Button();
            _hoverPanel.ShowAction2Button();
        }
        else
        {
            _hoverPanel.HideAction1Button();
            _hoverPanel.HideAction2Button();
        }
    }
}
