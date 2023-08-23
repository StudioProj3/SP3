using System.Collections.Generic;

using TMPro;
using UnityEngine;

using static DebugUtils;

public class UIMainInventory : MonoBehaviour
{
    [SerializeField]
    private CharacterData _character;

    [SerializeField]
    private GameObject _slotPrefab;

    private GameObject _viewport;
    private Transform _contentItems;
    private GameObject _noMainInventory;
    private List<UIItemSlot> _slots = new();

    private void Update()
    {
        InventoryBase inventory = _character.Inventory;

        _viewport.SetActive(inventory);
        _noMainInventory.SetActive(!inventory);

        if (!inventory)
        {
            return;
        }

        UpdateSlots(inventory);

        for (int i = 0; i < _slots.Count; ++i)
        {
            UIItemSlot slot = _slots[i];

            // There are more `_slots` than `MaxNumSlots`
            if (i >= inventory.MaxNumSlots)
            {
                // Hide this slot
                slot.gameObject.SetActive(false);

                continue;
            }

            // Show this slot
            slot.gameObject.SetActive(true);

            ItemBase item = inventory.GetItem(i);

            if (item)
            {
                slot.SetIconAndQuantity(item.Sprite,
                    inventory.GetAmount(i));
            }
            else
            {
                slot.SetIconAndQuantity(null, 0);
            }
        }
    }

    private void Awake()
    {
        _viewport = transform.ChildGO(0);
        _contentItems = _viewport.transform.GetChild(1);

        _slots.Capacity = _contentItems.childCount;
        for (int i = 0; i < _contentItems.childCount; ++i)
        {
            _slots.Add(_contentItems.GetChild(i).
                GetComponent<UIItemSlot>());
        }

        _noMainInventory = transform.ChildGO(1);
    }

    // Add slots needed and setup the references
    private void UpdateSlots(InventoryBase inventory)
    {
        Assert(inventory != null,
            "`inventory` should not be null");

        while (_slots.Count < inventory.MaxNumSlots)
        {
            GameObject newObject =
                Instantiate(_slotPrefab, _contentItems.transform);

            _slots.Add(newObject.GetComponent<UIItemSlot>());
        }
    }
}
