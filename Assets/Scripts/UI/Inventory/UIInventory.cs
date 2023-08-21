using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    [SerializeField]
    private CharacterData _character;

    private GameObject _content;
    private Transform _contentItems;
    private Image[] _slots;
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

        for (int i = 0; i < _slots.Length; ++i)
        {
            ItemBase item = inventory.GetItem(i);

            if (item)
            {
                _slots[i].sprite = item.Sprite;
            }
        }
    }

    private void Awake()
    {
        _content = transform.GetChild(0).gameObject;
        _contentItems = _content.transform.GetChild(1);

        _slots = new Image[_contentItems.childCount];
        for (int i = 0; i < _contentItems.childCount; ++i)
        {
            _slots[i] = _contentItems.GetChild(i, 0).
                GetComponent<Image>();
        }

        _noMainInventory = transform.GetChild(1).gameObject;
    }
}
