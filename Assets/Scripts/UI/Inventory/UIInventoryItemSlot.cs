using System;

using UnityEngine;
using UnityEngine.EventSystems;

using static DebugUtils;

public class UIInventoryItemSlot :
    UIItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField]
    public string Tag { get; private set; }

    [field: SerializeField]
    public InventoryBase Inventory { get; private set; }

    [SerializeField]
    [Range(-500f, 500f)]
    private float _offsetX = 0f;

    [SerializeField]
    [Range(-500f, 500f)]
    private float _offsetY = 0f;

    private RectTransform _rectTransform;
    private UIInventory _uiinventory;
    private UICrafting _uicrafting;
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
            _hoverPanel.HidePanel(), 0.1f);

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


        _rectTransform = GetComponent<RectTransform>();

        if (Tag == "UIInventory")
        {
            _uiinventory = GameObject.FindWithTag(Tag).
                GetComponent<UIInventory>();
            _hoverPanel = _uiinventory.transform.GetChild(2).
                GetComponent<UIHoverPanel>();
        }
        else if (Tag == "UICrafting")
        {
            _uicrafting = GameObject.FindWithTag(Tag).
                GetComponent<UICrafting>();
            _hoverPanel = _uicrafting.transform.GetChild(2).
                GetComponent<UIHoverPanel>();
        }
        else
        {
            Fatal("Unhandled tag type");
        }
    }

    private void UpdateHoverPanel()
    {
        ItemBase item = Inventory.GetItem(
            transform.GetSiblingIndex());

        _item = item;

        Pair<string, string> result = new();
        result.First = item != null ? item.Name : "Empty";
        result.Second = item != null ? item.Description : "-";

        _hoverPanel.SetEventArgs(item, transform.GetSiblingIndex(), Inventory);

        _hoverPanel.SetItemName(result.First);
        _hoverPanel.SetItemDescription(result.Second);

        _hoverPanel.Action1Button(item && item.Usable);
        _hoverPanel.Action2Button(item && item.Droppable);

        UpdateHoverPanelPosition();
    }

    private void UpdateHoverPanelPosition()
    {
        Vector2 position = _rectTransform.transform.position;
        position.x += _offsetX;
        position.y += _offsetY;

        _hoverPanel.ChangePosition(position);
    }
}
