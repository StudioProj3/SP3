using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using static DebugUtils;

public class UIMainInventory : MonoBehaviour
{
    [SerializeField]
    private CharacterData _character;

    [SerializeField]
    private GameObject _slotPrefab;

    private GameObject _content;
    private Transform _contentItems;
    private List<Image> _slots = new();
    private GameObject _noMainInventory;

    private void Update()
    {
        InventoryBase inventory = _character.Inventory;

        _content.SetActive(inventory);
        _noMainInventory.SetActive(!inventory);

        if (!inventory)
        {
            return;
        }

        UpdateSlots(inventory);

        for (int i = 0; i < _slots.Count; ++i)
        {
            GameObject parent = _slots[i].transform.parent.
                gameObject;

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
                _slots[i].sprite = item.Sprite;
                _slots[i].color = _slots[i].color.
                    Set(a: _slots[i].sprite ? 1f : 0f);
            }
        }
    }

    private void Awake()
    {
        _content = transform.GetChild(0).gameObject;
        _contentItems = _content.transform.GetChild(1);

        for (int i = 0; i < _contentItems.childCount; ++i)
        {
            _slots.Add(_contentItems.GetChild(i, 0).
                GetComponent<Image>());
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
            Image image = newObject.transform.GetChild(0).
                GetComponent<Image>();

            _slots.Add(image);
        }
    }
}
