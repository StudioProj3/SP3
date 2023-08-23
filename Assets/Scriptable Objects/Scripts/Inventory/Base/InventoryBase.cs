using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;

public abstract class InventoryBase :
    ScriptableObject
{
    public abstract uint MaxNumSlots { get; protected set; }

    public abstract uint MaxPerSlot { get; protected set; }

    [SerializeField]
    protected List<Pair<ItemBase, uint>> _itemInitializerList = new();

    protected List<Pair<ItemBase, uint>> _allItems = new();

    protected List<Pair<int, uint>> _indexToQuantityMap = new();
    protected List<Pair<int, bool>> _nonStackableIndexToNewValueMap =
        new();

    public int GetAmount(ItemBase itemBase)
    {
        int amount = 0;

        for (int i = 0; i < _allItems.Count; ++i)
        {
            if (_allItems[i].Key == itemBase)
            {
                amount += (int)_allItems[i].Value;
            }
        }

        return amount;
    }

    public uint GetAmount(int index)
    {
        return _allItems[index].Value;
    }

    public virtual void Reset()
    {
        // Clear any items that might still be in there
        for (int i = 0; i < _allItems.Capacity; ++i)
        {
            _allItems[i] = null;
            _itemInitializerList[i] = null;
        }

        _indexToQuantityMap.Clear();
        _nonStackableIndexToNewValueMap.Clear();
    }

    public virtual ItemBase GetItem(int index)
    {
        return _allItems[index]?.Key;
    }

    public virtual bool RemoveItemByIndex(int index, uint amount)
    {
        Assert(index >= 0, "`index` is less than zero");
        Assert(amount <= _allItems[index].Value,
             "`amount` is greater than item count");

        _allItems[index].Value -= amount;

        if (_allItems[index].Value == 0)
        {
            _allItems[index] = null;
        }
        return true;
    }

    // Function returns whether the modification request is valid
    // and caches the data needed for the modification
    protected virtual bool RequestModify(ItemBase item, int number)
    {
        bool isAdd = number > 0;
        int numberLeft = number;

        bool CannotAddOrRemove()
        {
            return (isAdd && numberLeft <= 0) ||
                (!isAdd && numberLeft >= 0);
        }

        // Loop through all the slots available
        for (int i = 0; i < MaxNumSlots; ++i)
        {
            // If all adding or removing was done
            if (CannotAddOrRemove())
            {
                return true;
            }

            bool isNull = _allItems[i] == null; 
            ItemBase currentItem = null;
            uint quantity = 0;

            if (!isNull)
            {
                currentItem = _allItems[i].Key;
                quantity = _allItems[i].Value;
            }

            if (isAdd)
            {
                if (item.Stackable)
                {
                    if (!isNull)
                    {
                        // Check if `item` is the not that same as
                        // `currentItem`, if so continue to next
                        // iteration
                        if (item != currentItem)
                        {
                            continue;
                        }

                        // If the current slot is already full move
                        // to the next iteration
                        if (quantity >= MaxPerSlot)
                        {
                            continue;
                        }

                        // Get the difference that we are able to fill up
                        int difference = (int)(MaxPerSlot - quantity);
                        
                        // If there is more than the difference, it will
                        // use the difference. If there is less than the
                        // difference, it will use the remainder
                        _indexToQuantityMap.Add(new Pair<int, uint>(i, 
                            (uint)(Mathf.Min(numberLeft,
                            difference) + quantity)));

                        // Hence, all we need to do is just minus the
                        // difference, which will be the max 'difference'
                        numberLeft -= difference;
                    }
                    else
                    {
                        // Either get the max per slot or remainder
                        int newValue = Mathf.Min((int)MaxPerSlot,
                            numberLeft);

                        // Use the new value obtained
                        _indexToQuantityMap.Add(
                            new Pair<int, uint>(i, (uint)newValue));
                        numberLeft -= newValue;    
                    }
                }
                else
                {
                    // There is already an item in
                    // this slot, move on to the next iteration
                    if (!isNull)
                    {
                        continue;
                    }

                    // Since it's an empty slot, we can
                    // just add to it
                    _nonStackableIndexToNewValueMap.Add(
                        new Pair<int, bool>(i, true));
                    numberLeft--;
                }
            }
            else
            {
                // Handle all the cases for removal

                // The slot is already empty. Continue to next iteration.
                // For the second condition, we already know that it
                // is not null since it will have to had been false to go
                // to the other condition in the Or statement
                if (isNull || currentItem != item)
                {
                    continue;
                }

                if (currentItem.Stackable)
                {
                    // New quantity will always be >0.
                    // We want to get the new quantity after subtracting
                    // what is left of the modification amount
                    int newQuantity = Mathf.Max(0,
                        (int)(quantity - Mathf.Abs(numberLeft)));

                    // Cache the result.
                    _indexToQuantityMap.Add(new Pair<int, uint>(i,
                        (uint)newQuantity));
                    numberLeft += (int)MaxPerSlot - newQuantity;
                }
                else
                {
                    // Since this item is not stackable, we
                    // can immediately remove this item on modify

                    // Cache the result
                    _nonStackableIndexToNewValueMap.Add(
                        new Pair<int, bool>(i, false));
                    numberLeft++;
                }
            }
        }

        // If we can still add or remove,
        // modification is rejected
        return CannotAddOrRemove();
    }

    // Add or remove items from the inventory based on the sign
    // of `number` with it returning whether the operation was
    // a success
    protected virtual bool Modify(ItemBase item, int number,
        bool request = true)
    {
        Assert(number != 0, "`number` is zero");

        // If request is false, check that one of the maps
        // have at least one thing so we know there is a
        // modification cached
        // If request is true, then run the `RequestModify` function
        if ((!request && (!_indexToQuantityMap.IsEmpty()
            || !_nonStackableIndexToNewValueMap.IsEmpty()))
            || (request && RequestModify(item, number)))
        {
            _indexToQuantityMap.ForEach(i => 
            {
                if (_allItems[i.Key] == null)
                {
                    // The item slot is empty, create a new
                    // item with its new quantity
                    _allItems[i.Key] = new Pair<ItemBase, uint>
                        (item, i.Value);
                    _itemInitializerList[i.Key] = _allItems[i.Key];
                }
                else
                {
                    // The same item type already exists, just
                    // modify the existing item in the item slot
                    // However, if the new value is 0, we should
                    // empty the slot
                    if (i.Value > 0)
                    {
                        _allItems[i.Key].Value = i.Value;
                        _itemInitializerList[i.Key].Value = i.Value;
                    }
                    else
                    {
                        _allItems[i.Key] = null;
                        _itemInitializerList[i.Key] = null;
                    }
                }
            });

            _nonStackableIndexToNewValueMap.ForEach(i => 
            {
                if (i.Value)
                {
                    // We want to import this stackable item
                    if (_allItems[i.Key] != null)
                    {
                        // Just modify the item when it already exists
                        _allItems[i.Key].Value = 1;
                        _itemInitializerList[i.Key].Value = 1;
                    }
                    else
                    {
                        // Create the new missing item
                        _allItems[i.Key] = new Pair<ItemBase, uint>
                            (item, 1);
                        _itemInitializerList[i.Key] = _allItems[i.Key];
                    }
                }
                else
                {
                    // We are exporting this stackable item, and
                    // since there's only one of it, we can just
                    // empty the slot out
                    _allItems[i.Key] = null;
                    _itemInitializerList[i.Key] = null;
                }
            });

            _indexToQuantityMap.Clear();
            _nonStackableIndexToNewValueMap.Clear();

            return true;
        }

        return false;
    }

    public virtual bool Add(ItemBase item, uint number)
    {
        return Modify(item, (int)number);
    }

    public virtual bool Remove(ItemBase item, uint number)
    {
        return Modify(item, (int)(number * -1));
    }

    // For debugging
    public void Print()
    {
        Log("Inventory size: "  + _allItems.Count.ToString());

        for (int i = 0; i < _allItems.Count; ++i)
        {
            if (_allItems[i] != null) 
            {
                Log("Index " + i.ToString() + " : " +
                    _allItems[i].First.Name + 
                    " : " + _allItems[i].Second.ToString()); 
            }
        }
    }

    // Update the inventory list `_allItems` with
    // the starter items and new max number of slots
    protected virtual void OnValidate()
    {
        bool InitializerItemExists(Pair<ItemBase, uint> pair) =>
            pair != null && pair.First != null;

        bool ItemExists(Pair<ItemBase, uint> pair) =>
            pair != null && pair.First != null && pair.Second != 0;

        // If the new max number of slots is below the current count,
        // start removing from the end
        // else, add nulls to the back
        //
        // NOTE (Chris): By adding null, you are just adding a new pair with
        // null values
        if (MaxNumSlots < _itemInitializerList.Count)
        {
            while (_itemInitializerList.Count > MaxNumSlots)
            {
                _itemInitializerList.
                    RemoveAt(_itemInitializerList.Count - 1);
            }
        }
        else
        {
            while (_itemInitializerList.Count < MaxNumSlots)
            {
                _itemInitializerList.Add(null);
            }
        }

        // If the initializer item is valid keep it, basically have at least
        // the item base scriptable object, but then will be validated
        // later at the later for loop that adds it
        // If it is not, just set it to null, which will empty out the fields
        for (int i = 0; i < _itemInitializerList.Count; ++i)
        {
            if (!InitializerItemExists(_itemInitializerList[i]))
            {
                _itemInitializerList[i] = null;
            }
        }

        _itemInitializerList.Capacity = (int)MaxNumSlots;
        
        // Clear the internal item list
        _allItems.Clear();

        // Initialize the internal item list
        for (int i = 0; i < _itemInitializerList.Count; ++i)
        {
            _allItems.Add(ItemExists(_itemInitializerList[i]) ? 
                _itemInitializerList[i] : null);
        }
    }
}
