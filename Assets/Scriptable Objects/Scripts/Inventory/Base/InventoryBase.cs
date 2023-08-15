using System.Collections.Generic;

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

    protected List<Pair<ItemBase, uint>> _allItems;

    // Add or remove items from the inventory based on the sign
    // of `number` with it returning whether the operation was
    // a success
    protected virtual bool Modify(ItemBase item, int number)
    {
        return true;
    }

    protected virtual bool Add(ItemBase item, uint number)
    {
        return Modify(item, (int)number);
    }

    protected virtual bool Remove(ItemBase item, uint number)
    {
        return Modify(item, (int)(number * -1));
    }
}
