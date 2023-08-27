using System.Collections.Generic;

using UnityEngine;

public class BasicCraft : MonoBehaviour
{
    [SerializeField]
    private List<BasicRecipe> _allRecipes = new();

    public bool Craft(BasicRecipeInventory inventory,
        UnitInventory unit)
    {
        uint freeSpace = unit.MaxPerSlot - unit.SlotAmount();

        foreach (BasicRecipe recipe in _allRecipes)
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

    public bool Match(BasicRecipeInventory inventory,
        BasicRecipe recipe)
    {
        return inventory.Row1Col1() == recipe.Row1Col1 &&
            inventory.Row1Col2() == recipe.Row1Col2 &&
            inventory.Row2Col1() == recipe.Row2Col1 &&
            inventory.Row2Col2() == recipe.Row2Col2;
    }

    private void Deduct(BasicRecipeInventory inventory,
        BasicRecipe recipe)
    {
        ItemBase[] arr = new ItemBase[]
        {
            recipe.Row1Col1, recipe.Row1Col2,
            recipe.Row2Col1, recipe.Row2Col2,
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
