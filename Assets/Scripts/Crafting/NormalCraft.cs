using System.Collections.Generic;

using UnityEngine;

public class NormalCraft : MonoBehaviour
{
    [SerializeField]
    private List<NormalRecipe> _allRecipes = new();

    public bool Craft(NormalRecipeInventory inventory,
        UnitInventory unit)
    {
        uint freeSpace = unit.MaxPerSlot - unit.SlotAmount();

        foreach (NormalRecipe recipe in _allRecipes)
        {
            uint spaceNeeded = recipe.TargetQuantity;

            // Not enough space in the target `UnitInventory`
            if (spaceNeeded > freeSpace)
            {
                continue;
            }

            // Items in `inventory` does not match the current
            // `recipe`
            if (!Match(inventory, recipe))
            {
                continue;
            }

            // All conditions are met, perform the craft
            Deduct(inventory, recipe);
            unit.Add(recipe.Target, spaceNeeded);

            return true;
        }

        return false;
    }

    public bool Match(NormalRecipeInventory inventory,
        NormalRecipe recipe)
    {
        return inventory.Row1Col1() == recipe.Row1Col1 &&
            inventory.Row1Col2() == recipe.Row1Col2 &&
            inventory.Row1Col3() == recipe.Row1Col3 &&
            inventory.Row2Col1() == recipe.Row2Col1 &&
            inventory.Row2Col2() == recipe.Row2Col2 &&
            inventory.Row2Col3() == recipe.Row2Col3 &&
            inventory.Row3Col1() == recipe.Row3Col1 &&
            inventory.Row3Col2() == recipe.Row3Col2 &&
            inventory.Row3Col3() == recipe.Row3Col3;
    }

    private void Deduct(NormalRecipeInventory inventory,
        NormalRecipe recipe)
    {
        ItemBase[] arr = new ItemBase[]
        {
            recipe.Row1Col1, recipe.Row1Col2, recipe.Row1Col3,
            recipe.Row2Col1, recipe.Row2Col2, recipe.Row2Col3,
            recipe.Row3Col1, recipe.Row3Col2, recipe.Row3Col3,
        };

        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i])
            {
                inventory.RemoveItemByIndex(i, 1);
            }
        }
    }
}
