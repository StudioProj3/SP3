using System.Collections.Generic;

using UnityEngine;

public class NormalCraft : MonoBehaviour
{
    [field: SerializeField]
    public List<NormalRecipe> AllRecipes { get; private set; }

    private Animator _animator;

    public bool Craft(NormalRecipeInventory inventory,
        UnitInventory unit)
    {
        uint freeSpace = unit.MaxPerSlot - unit.SlotAmount();

        foreach (NormalRecipe recipe in AllRecipes)
        {
            uint spaceNeeded = recipe.TargetQuantity;

            // Target item is not the same hence stacking
            // is not possible
            if (unit.Slot() && recipe.Target != unit.Slot())
            {
                continue;
            }

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

        Shake();

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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Shake()
    {
        _animator.SetTrigger("shake");
    }
}
