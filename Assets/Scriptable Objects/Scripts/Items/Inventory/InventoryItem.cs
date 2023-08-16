using UnityEngine;

[CreateAssetMenu(fileName = "Inventory",
    menuName = "Items/Inventory")]
public class InventoryItem : ItemBase
{
    [field: SerializeField]
    public InventoryBase Inventory { get; private set; }
}
