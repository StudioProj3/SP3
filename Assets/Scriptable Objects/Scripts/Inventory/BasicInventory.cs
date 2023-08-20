using UnityEngine;

[CreateAssetMenu(fileName = "Inventory",
    menuName = "Scriptable Objects/Inventory/BasicInventory")]
public class BasicInventory : InventoryBase
{
    [field: SerializeField]
    public override uint MaxNumSlots { get; protected set; }

    [field: SerializeField]
    public override uint MaxPerSlot { get; protected set; }
}
