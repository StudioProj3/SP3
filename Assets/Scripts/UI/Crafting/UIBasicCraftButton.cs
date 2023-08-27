using UnityEngine;
using UnityEngine.UI;

public class UIBasicCraftButton : MonoBehaviour
{
    private BasicCraft _basicCraft;
    private BasicRecipeInventory _inventory;
    private UnitInventory _unit;
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            _basicCraft.Craft(_inventory, _unit);
        });
    }

    private void Awake()
    {
        Transform parent = transform.parent;

        _basicCraft = GetComponentInParent<BasicCraft>();

        Transform inventoryRoot = parent.GetChild(0);
        Transform unitRoot = parent.GetChild(1);

        _inventory = (BasicRecipeInventory)inventoryRoot.
            GetChild(0, 0, 0).GetComponent<UIInventoryItemSlot>().
            Inventory;
        _unit = (UnitInventory)unitRoot.GetChild(0, 0, 0).
            GetComponent<UIInventoryItemSlot>().Inventory;

        _button = GetComponent<Button>();
    }
}
