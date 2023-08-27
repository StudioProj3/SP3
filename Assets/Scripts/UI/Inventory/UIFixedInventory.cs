using UnityEngine;

public class UIFixedInventory : MonoBehaviour
{
    [SerializeField]
    private InventoryBase _inventory;

    private Transform _contentItems;
    private UIInventoryItemSlot[] _allSlots;

    private void Update()
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

    private void Awake()
    {
        _contentItems = transform.GetChild(0, 0);

        uint numSlots = _inventory.MaxNumSlots;
        _allSlots = new UIInventoryItemSlot[numSlots];
       
        for (int i = 0; i < numSlots; ++i)
        {
            _allSlots[i] = _contentItems.GetChild(i).
                GetComponent<UIInventoryItemSlot>();
        }
    }
}
