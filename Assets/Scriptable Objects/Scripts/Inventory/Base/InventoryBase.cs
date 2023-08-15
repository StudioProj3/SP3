using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;

public abstract class InventoryBase :
    ScriptableObject
{
    public uint MaxNumSlots
    {
        get => _maxNumSlots;
        protected set => _maxNumSlots = value;
    }

    public uint MaxPerSlot
    {
        get => _maxPerSlot;
        protected set => _maxPerSlot = value;
    }

    [SerializeField]
    [Tooltip("Number of slots in the inventory")]
    protected uint _maxNumSlots;

    [SerializeField]
    [Tooltip("Max number of items in 1 slot")]
    protected uint _maxPerSlot;

    [SerializeField]
    protected List<Pair<ItemBase, uint>> _allItems = new();

    protected Dictionary<int, int> _indexToQuantityMap = new();

    // Function returns whether the modification request is valid. It caches the data
    // needed for the modification.
    protected virtual bool RequestModify(ItemBase item, int number)
    {
        Assert.IsTrue(number != 0,
            "`number` is zero");

        bool isAdd = number > 0;
        int numberLeft = number;
        bool isItemStackable = item.Stackable;

        // Loop through all the slots available
        for (int i = 0; i < _maxNumSlots; ++i)
        {
            // If all adding or removing was done
            if ((isAdd && numberLeft <= 0) ||
                (!isAdd && numberLeft >= 0))
            {
                break;
            }

            ItemBase currentItem = _allItems[i].Key;
            uint quantity = _allItems[i].Value;
            bool isNull = currentItem == null;
            bool isCurrentStackable = currentItem.Stackable;

            // Check if `item` is the not that same as
            // `currentItem`, if so continue to next iteration
            if (item != currentItem)
            {
                continue;
            }

            if (isAdd)
            {
                // There is already an unstackable item in
                // this slot, move on to the next iteration
                if (!isNull && !currentItem.Stackable)
                {
                    continue;
                }

                // If the current slot is already full move
                // to the next iteration
                if (quantity >= _maxPerSlot)
                {
                    continue;
                }

                // TODO (Cheng Jun): Continue adding item codes

                // Get the difference that we are able to fill up.
                int difference = (int)(_maxPerSlot - quantity);
                
                // Check if we can fill up the slot fully.
                if (numberLeft >= difference)
                {
                    _indexToQuantityMap.Add(i, difference);
                    numberLeft -= difference;
                }
                
            }
            else
            {
                // TODO (Cheng Jun): Continue removing item codes
            }
        }

        return true;
        
    }

    // Add or remove items from the inventory based on the sign
    // of `number` with it returning whether the operation was
    // a success
    protected virtual bool Modify(ItemBase item, int number)
    {
        Assert.IsTrue(number != 0,
            "`number` is zero");

        bool isAdd = number > 0;
        int numberLeft = number;
        bool isItemStackable = item.Stackable;

        // Loop through all the slots available
        for (int i = 0; i < _maxNumSlots; ++i)
        {
            // If all adding or removing was done
            if ((isAdd && numberLeft <= 0) ||
                (!isAdd && numberLeft >= 0))
            {
                break;
            }

            ItemBase currentItem = _allItems[i].Key;
            uint quantity = _allItems[i].Value;
            bool isNull = currentItem == null;
            bool isCurrentStackable = currentItem.Stackable;

            // Check if `item` is the not that same as
            // `currentItem`, if so continue to next iteration
            if (item != currentItem)
            {
                continue;
            }

            if (isAdd)
            {
                // There is already an unstackable item in
                // this slot, move on to the next iteration
                if (!isNull && !currentItem.Stackable)
                {
                    continue;
                }

                // If the current slot is already full move
                // to the next iteration
                if (quantity >= _maxPerSlot)
                {
                    continue;
                }

                // TODO (Cheng Jun): Continue adding item codes
            }
            else
            {
                // TODO (Cheng Jun): Continue removing item codes
            }
        }

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

    // Update the inventory list `_allItems` such that the
    // `Count` matches that of `_maxNumSlots` with empty
    // elements initialized to `null`
    protected virtual void OnValidate()
    {
        _allItems.Capacity = (int)_maxNumSlots;

        while (_allItems.Count < _maxNumSlots)
        {
            _allItems.Add(null);
        }
    }
}
