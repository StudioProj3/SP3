using UnityEngine;

public class UIFixedInventory : MonoBehaviour
{
    public enum Type
    {
        Basic,
        Normal,
    }

    [SerializeField]
    private Type _type;

    [SerializeField]
    private InventoryBase _inventory;

    private UICrafting _uicrafting;
    private UIBasicCrafting _uibasicCrafting;
    private UINormalCrafting _uinormalCrafting;
    private Transform _contentItems;
    private UIInventoryItemSlot[] _allSlots;

    private void Update()
    {
        if (_uicrafting.CurrentMode == UICrafting.Mode.Craft)
        {
            for (int i = 0; i < _allSlots.Length; ++i)
            {
                UIInventoryItemSlot slot = _allSlots[i];
                ItemBase item = _inventory.GetItem(i);
                uint amount = _inventory.GetAmount(i);

                if (_inventory.GetItem(i))
                {
                    slot.SetIconAndQuantity(item.Sprite,
                        amount);
                }
                else
                {
                    slot.SetIconAndQuantity(null, 0);
                }
            }
        }
        else
        {
            if (_type == Type.Basic)
            {
                BasicRecipe recipe = _uibasicCrafting.
                    GetRecipe();

                if (_allSlots.Length == 4)
                {
                    SetSlot(0, recipe.Row1Col1);
                    SetSlot(1, recipe.Row1Col2);
                    SetSlot(2, recipe.Row2Col1);
                    SetSlot(3, recipe.Row2Col2);
                }
                else
                {
                    _allSlots[0].SetIconAndQuantity(recipe.Target.
                        Sprite, recipe.TargetQuantity);
                }
            }
            else if (_type == Type.Normal)
            {
                NormalRecipe recipe = _uinormalCrafting.
                    GetRecipe();

                if (_allSlots.Length == 9)
                {
                    SetSlot(0, recipe.Row1Col1);
                    SetSlot(1, recipe.Row1Col2);
                    SetSlot(2, recipe.Row1Col3);
                    SetSlot(3, recipe.Row2Col1);
                    SetSlot(4, recipe.Row2Col2);
                    SetSlot(5, recipe.Row2Col3);
                    SetSlot(6, recipe.Row3Col1);
                    SetSlot(7, recipe.Row3Col2);
                    SetSlot(8, recipe.Row3Col3);
                }
                else
                {
                    _allSlots[0].SetIconAndQuantity(recipe.Target.
                        Sprite, recipe.TargetQuantity);
                }
            }
        }
    }

    private void Awake()
    {
        _uicrafting = GameObject.FindWithTag("UICrafting").
            GetComponent<UICrafting>();
        _contentItems = transform.GetChild(0, 0);

        if (_type == Type.Basic)
        {
            _uibasicCrafting =
                GetComponentInParent<UIBasicCrafting>();
        }
        else if (_type == Type.Normal)
        {
            _uinormalCrafting =
                GetComponentInParent<UINormalCrafting>();
        }

        uint numSlots = _inventory.MaxNumSlots;
        _allSlots = new UIInventoryItemSlot[numSlots];
       
        for (int i = 0; i < numSlots; ++i)
        {
            _allSlots[i] = _contentItems.GetChild(i).
                GetComponent<UIInventoryItemSlot>();
        }
    }

    private void SetSlot(int index, ItemBase item,
        uint count = 1)
    {
        if (item)
        {
            _allSlots[index].SetIconAndQuantity(
                item.Sprite, count);
        }
        else
        {
            _allSlots[index].SetIconAndQuantity(
                null, 0);
        }
    }
}
