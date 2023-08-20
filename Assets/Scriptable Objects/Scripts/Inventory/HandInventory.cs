using UnityEngine;

[CreateAssetMenu(fileName = "HandInventory",
    menuName = "Scriptable Objects/Inventory/HandInventory")]
public class HandInventory :
    InventoryBase
{
    public override uint MaxNumSlots { get; protected set; } = 2;

    public override uint MaxPerSlot { get; protected set; } = 1;

    public override void Reset()
    {
        base.Reset();
    }

    protected override void OnValidate()
    {
        MaxNumSlots = 2;
        MaxPerSlot = 1;

        base.OnValidate();
    }

    private void Awake()
    {
        OnValidate();
    }
}
