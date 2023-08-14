using UnityEngine;

public abstract class InventoryBase :
    ScriptableObject
{
    public uint NumSlots
    {
        get => _numSlots;
        protected set => _numSlots = value;
    }

    public uint MaxPerSlot
    {
        get => _maxPerSlot;
        protected set => _maxPerSlot = value;
    }

    [SerializeField]
    [Tooltip("Number of slots in the inventory")]
    protected uint _numSlots;

    [SerializeField]
    [Tooltip("Max number of items in 1 slot")]
    protected uint _maxPerSlot;
}
