using System.Collections.Generic;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private List<Pair<Image, TMP_Text>> _slots = new();

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
            Image image = _slots[i].First;
            TMP_Text text = _slots[i].Second;

            GameObject parent = image.transform.parent.gameObject;

            // There are more `_slots` than `MaxNumSlots`
            if (i >= inventory.MaxNumSlots)
            {
                // Hide this slot
                parent.SetActive(false);

                continue;
            }

            // Show this slot
            parent.SetActive(true);

            ItemBase item = inventory.GetItem(i);

            if (item)
            {
                // Update item icon
                image.sprite = item.Sprite;
                image.color = image.color.
                    Set(a: image.sprite ? 1f : 0f);

                // Update item quantity
                // TODO (Cheng Jun): Complete item
                // quantity implementation
            }
        }
    }

    private void Awake()
    {
        _viewport = transform.GetChild(0).gameObject;
        _contentItems = _viewport.transform.GetChild(1);

        for (int i = 0; i < _contentItems.childCount; ++i)
        {
            AddSlot(_contentItems.GetChild(i));
        }

        _noMainInventory = transform.GetChild(1).gameObject;
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

            AddSlot(newObject.transform);
        }
    }

    private void AddSlot(Transform slot)
    {
        Image image = slot.GetChild(0).
            GetComponent<Image>();
        TMP_Text text = slot.GetChild(1).
            GetComponent<TMP_Text>();

        _slots.Add(new(image, text));
    }
}
