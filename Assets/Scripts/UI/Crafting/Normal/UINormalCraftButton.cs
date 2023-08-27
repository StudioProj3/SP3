using UnityEngine;
using UnityEngine.UI;

public class UINormalCraftButton : MonoBehaviour
{
    private NormalCraft _normalCraft;
    private NormalRecipeInventory _inventory;
    private UnitInventory _unit;
    private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            _normalCraft.Craft(_inventory, _unit);
        });
    }

    private void Awake()
    {
        Transform parent = transform.parent;

        _normalCraft = GetComponentInParent<NormalCraft>();

        Transform inventoryRoot = parent.GetChild(0);
        Transform unitRoot = parent.GetChild(1);

        _inventory = (NormalRecipeInventory)inventoryRoot.
            GetChild(0, 0, 0).GetComponent<UIInventoryItemSlot>().
            Inventory;
        _unit = (UnitInventory)unitRoot.GetChild(0, 0, 0).
            GetComponent<UIInventoryItemSlot>().Inventory;

        _button = GetComponent<Button>();
    }
}
