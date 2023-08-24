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

    // Convenience helper function for readability
    public ItemBase LeftHand()
    {
        return GetItem(0);
    }

    // Convenience helper function for readability
    public ItemBase RightHand()
    {
        return GetItem(1);
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
