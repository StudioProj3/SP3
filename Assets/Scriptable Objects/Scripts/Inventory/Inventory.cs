using UnityEngine;

[CreateAssetMenu(fileName = "Inventory",
    menuName = "Scriptable Objects/Inventory/Inventory")]
public class Inventory : InventoryBase
{
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

}
