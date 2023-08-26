using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using static DebugUtils;

public class UIHoverPanel :
    MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<ItemBase, int, InventoryBase> OnItemUse;
    public static event Action<ItemBase, int, InventoryBase> OnItemDrop;
    // Prevent closing of this hover panel
    private bool _lock = false;

    // Prevent the hover panel from begin visible,
    // overrides `_lock`
    private bool _hidden = false;

    private RectTransform _rectTransform;
    private TMP_Text _itemName;
    private TMP_Text _itemDescription;
    private GameObject _action1Button;
    private GameObject _action2Button;

    private ItemBase _referenceItem;
    private int _itemIndex;
    private InventoryBase _refInventory;

    public void MakeHidden()
    {
        _hidden = true;
    }

    public void MakeVisible()
    {
        _hidden = false;
    }

    public void ShowPanel()
    {
        // Only allow changes if `_lock` is false
        if (!_lock && !_hidden)
        {
            gameObject.SetActive(true);
        }
    }

    public void HidePanel()
    {
        // Only allow changes if `_lock` is false
        if (!_lock)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnUseButtonClick()
    {
        if (_referenceItem == null)
        {
            return;
        }

        OnItemUse?.Invoke(_referenceItem, _itemIndex, _refInventory);
    }

    public void OnDropButtonClick()
    {
        if (_referenceItem == null)
        {
            return;
        }

        OnItemDrop?.Invoke(_referenceItem, _itemIndex, _refInventory);
    }

    public void SetEventArgs(ItemBase item, int index, 
        InventoryBase refInventory)
    {
        _referenceItem = item;
        _itemIndex = index;
        _refInventory = refInventory;
    }

    public void SetItemName(string name)
    {
        Assert(!name.IsNullOrEmpty(),
            "`name` should not be null or empty");

        // Only allow changes if `_lock` is false
        if (!_lock)
        {
            _itemName.text = name;
        }
    }

    public void SetItemDescription(string description)
    {
        Assert(!description.IsNullOrEmpty(),
            "`description` should not be null or empty");

        // Only allow changes if `_lock` is false
        if (!_lock)
        {
            _itemDescription.text = description;
        }
    }

    public void ShowAction1Button()
    {
        Action1Button(true);
    }

    public void HideAction1Button()
    {
        Action1Button(false);
    }

    public void Action1Button(bool active)
    {
        _action1Button.SetActive(active);
    }

    public void ShowAction2Button()
    {
        Action2Button(true);
    }

    public void HideAction2Button()
    {
        Action2Button(false);
    }

    public void Action2Button(bool active)
    {
        _action2Button.SetActive(active);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _lock = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _lock = false;
        HidePanel();
    }

    public void ChangePosition(Vector2 position)
    {
        float halfWidth = Screen.width / 2f;
        float halfHeight = Screen.height / 2f;

        _rectTransform.anchoredPosition =
            new(position.x - halfWidth,
            position.y - halfHeight);
    }

    private void Update()
    {
        if (_hidden)
        {
            gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _itemName = transform.GetChild(0).
            GetComponent<TMP_Text>();
        _itemDescription = transform.GetChild(1).
            GetComponent<TMP_Text>();

        _action1Button = transform.ChildGO(2);
        _action2Button = transform.ChildGO(3);
    }
}
