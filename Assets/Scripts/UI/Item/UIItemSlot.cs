using System;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{

    public static event Action<ItemBase, int> OnUseFromMainInventory;
    public static event Action<ItemBase, int> OnUseFromHandInventory;

    private TMP_Text _itemQuantity;
    protected int _index;
    protected bool _inMain;
    protected ItemBase _item;

    public UIItemSlot Init(int index, bool inMain, ItemBase item = null)
    {
        _index = index;
        _inMain = inMain;

        if (item != null)
        {
            _item = item; 
        }

        return this;
    }

    public void SetItem(ItemBase item)
    {
        _item = item;
    }
    public void SetIcon(Sprite sprite)
    {
        Transform icon = transform.GetChild(0);
        
        if (!icon)
        {
            return;
        }

        Image image = icon.GetComponent<Image>();

        if (!image)
        {
            return;
        }

        image.sprite = sprite;
        image.color = image.color.
            Set(a: sprite ? 1f : 0f);
    }

    public void SetQuantity(uint quantity)
    {
        _itemQuantity.text = quantity.ToString();
    }

    public void SetIconAndQuantity(Sprite sprite, uint quantity)
    {
        SetIcon(sprite);
        SetQuantity(quantity);
    }

    public void UseItemInSlot()
    {
        if (_inMain)
        {
            OnUseFromMainInventory(_item, _index);
        }
        else
        {
            Debug.Log("Item use");
            OnUseFromHandInventory(_item, _index);
        }
    }

    protected virtual void Awake()
    {
        _itemQuantity = transform.GetChild(1).
            GetComponent<TMP_Text>();
    }
}
