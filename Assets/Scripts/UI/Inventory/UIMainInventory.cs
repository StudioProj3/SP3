using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;

public class UIMainInventory : MonoBehaviour
{
    public InventoryBase Inventory { get; private set; }

    [SerializeField]
    private CharacterData _character;

    [SerializeField]
    private GameObject _slotPrefab;

    private GameObject _viewport;
    private Transform _contentItems;
    private GameObject _noMainInventory;
    private List<UIInventoryItemSlot> _slots = new();

    private void Update()
    {
        Inventory = _character.Inventory;

        _viewport.SetActive(Inventory);
        _noMainInventory.SetActive(!Inventory);

        if (!Inventory)
        {
            return;
        }

        UpdateSlots(Inventory);

        for (int i = 0; i < _slots.Count; ++i)
        {
            UIItemSlot slot = _slots[i];

            // There are more `_slots` than `MaxNumSlots`
            if (i >= Inventory.MaxNumSlots)
            {
                // Hide this slot
                slot.gameObject.SetActive(false);

                continue;
            }

            // Show this slot
            slot.gameObject.SetActive(true);

            ItemBase item = Inventory.GetItem(i);

            if (item)
            {
                slot.SetIconAndQuantity(item.Sprite,
                    Inventory.GetAmount(i));
            }
            else
            {
                slot.SetIconAndQuantity(null, 0);
            }
        }
    }

    private void Start()
    {
        _viewport.SetActive(_character.Inventory);
        _noMainInventory.SetActive(!_character.Inventory);
    }

    private void Awake()
    {
        _viewport = transform.ChildGO(0);
        _contentItems = _viewport.transform.GetChild(1);

        _slots.Capacity = _contentItems.childCount;
        for (int i = 0; i < _contentItems.childCount; ++i)
        {
            _slots.Add(_contentItems.GetChild(i).
                GetComponent<UIInventoryItemSlot>());
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

            _slots.Add(newObject.GetComponent<UIInventoryItemSlot>());
        }
    }
}
