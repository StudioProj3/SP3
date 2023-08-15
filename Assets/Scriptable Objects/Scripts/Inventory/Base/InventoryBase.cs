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

    protected List<Pair<int, uint>> _indexToQuantityMap = new();
    protected List<Pair<int, bool>> _nonStackableIndexToNewValueMap = new();

    // Function returns whether the modification request is valid. It caches the data
    // needed for the modification.
    protected virtual bool RequestModify(ItemBase item, int number)
    {
        Assert.IsTrue(number != 0,
            "`number` is zero");

        bool isAdd = number > 0;
        int numberLeft = number;
        bool isItemStackable = item.Stackable;

        bool CanAddOrRemove()
        {
            return (isAdd && numberLeft <= 0) ||
                (!isAdd && numberLeft >= 0);
        }

        // Loop through all the slots available
        for (int i = 0; i < _maxNumSlots; ++i)
        {
            // If all adding or removing was done
            if (!CanAddOrRemove())
            {
                return true;
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
                if (currentItem.Stackable)
                {
                    // If the current slot is already full move
                    // to the next iteration
                    if (quantity >= _maxPerSlot)
                    {
                        continue;
                    }
                    // Get the difference that we are able to fill up.
                    int difference = (int)(_maxPerSlot - quantity);
                    
                    // If there is more than the difference, it will use the
                    // difference. If there is less than the difference, it
                    // will use the remainder
                    _indexToQuantityMap.Add(new Pair<int, uint>(i, 
                        (uint)Mathf.Min(numberLeft,
                        (int)quantity + difference)));

                    // Hence, all we need to do is just minus the difference,
                    // which will be the max 'difference'
                    numberLeft -= difference;
                }
                else
                {
                    // There is already an item in
                    // this slot, move on to the next iteration
                    if (!isNull)
                    {
                        continue;
                    }

                    _nonStackableIndexToNewValueMap.Add(new Pair<int, bool>(i, true));
                    numberLeft--;
                }
            }
            else
            {
                // The slot is already empty. Continue to next iteration.
                if (isNull)
                {
                    continue;
                }

                if (currentItem.Stackable)
                {
                    // New quantity will always be >0.
                    // We want to get the new quantity after subtracting what
                    // is left of the modification amount.
                    int newQuantity = Mathf.Max(0, (int)(quantity - Mathf.Abs(numberLeft)));

                    // Cache the result.
                    _indexToQuantityMap.Add(new Pair<int, uint>(i,
                        (uint)newQuantity));
                    numberLeft += (int)_maxPerSlot - newQuantity;
                }
                else
                {
                    // Since this item is not stackable, we can immediately
                    // remove this item on modify.

                    // Cache the result.
                    _nonStackableIndexToNewValueMap.Add(
                        new Pair<int, bool>(i, false));
                    numberLeft++;
                }
            }
        }

        // If we can still add or remove, modification is rejected.
        return !CanAddOrRemove();
    }

    // Add or remove items from the inventory based on the sign
    // of `number` with it returning whether the operation was
    // a success
    protected virtual bool Modify(ItemBase item, int number, bool request = true)
    {
        if (!request || (request && RequestModify(item, number)))
        {
            _indexToQuantityMap.ForEach(i => 
            {
                if (_allItems[i.Key] == null)
                {
                    // The item slot is empty, create a new item with
                    // its new quantity.
                    _allItems[i.Key] = new Pair<ItemBase, uint>(item, i.Value);
                }
                else
                {
                    // The same item type already exists, just modify
                    // the existing item in the item slot.
                    _allItems[i.Key].Value = i.Value;
                }
            });

            _nonStackableIndexToNewValueMap.ForEach(i => 
            {
                if (i.Value)
                {
                    // We want to import this stackable item.
                    if (_allItems[i.Key] != null)
                    {
                        // Just modify the item when it already exists.
                        _allItems[i.Key].Value = 1;
                    }
                    else
                    {
                        // Create the new missing item.
                        _allItems[i.Key] = new Pair<ItemBase, uint>(item, 1);
                    }
                }
                else
                {
                    // We are exporting this stackable item, and since
                    // there's only one of it, we can just empty the slot out
                    _allItems[i.Key] = null;
                }
            });

            _indexToQuantityMap.Clear();
            _nonStackableIndexToNewValueMap.Clear();
            return true;
        }
        return false;
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
