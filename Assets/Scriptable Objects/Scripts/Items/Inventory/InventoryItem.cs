using UnityEngine;

[CreateAssetMenu(fileName = "Inventory",
    menuName = "Scriptable Objects/Items/Inventory")]
public class InventoryItem : ItemBase
{
    [field: HorizontalDivider]
    [field: Header("Inventory Parameters")]

    [field: SerializeField]
    public InventoryBase Inventory { get; private set; }

    public InventoryItem()
    {
        Name = "Inventory";
    }
}
