using System.Collections.Generic;

using UnityEngine;

public class BasicCraft : MonoBehaviour
{
    [field: SerializeField]
    public List<BasicRecipe> AllRecipes { get; private set; }

    private Animator _animator;

    public bool Craft(BasicRecipeInventory inventory,
        UnitInventory unit)
    {
        uint freeSpace = unit.MaxPerSlot - unit.SlotAmount();

        foreach (BasicRecipe recipe in AllRecipes)
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

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Shake()
    {
        _animator.SetTrigger("shake");
    }
}
