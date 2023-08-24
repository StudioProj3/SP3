using UnityEngine;
using UnityEngine.EventSystems;

public class UIInventoryItemSlot :
    UIItemSlot, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private InventoryBase _inventory;

    [SerializeField]
    [Range(-500f, 500f)]
    private float _offsetX = 0f;

    [SerializeField]
    [Range(-500f, 500f)]
    private float _offsetY = 0f;

    private RectTransform _rectTransform;
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
