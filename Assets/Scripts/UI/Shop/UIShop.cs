using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

using static DebugUtils;

public class UIShop : MonoBehaviour
{
    [SerializeField]
    private UIShopItem _itemPrefab; 

    // TODO (Chris): We can map specific lists to parent transforms so we
    // do not have to divide it up into unrolled fields.

    [HorizontalDivider]
    [Header("Content Holders")]
    [SerializeField]
    private Transform _materialsTransform;

    [HorizontalDivider]
    [SerializeField]
    private List<ShopItem> _materials = new();

    // TODO (Chris): Change ItemBase to CoinItemBase then test to allow static
    // type checking.

    [HorizontalDivider]
    [Header("Currencies in use")]
    [SerializeField]
    private ItemBase _bronzeCoin;

    [SerializeField]
    private ItemBase _silverCoin;

    [SerializeField]
    private ItemBase _goldCoin;

    // NOTE (Chris): I have these unrolled to make sure they are in order.

    private void OnValidate()
    {
    }

    private void Start()
    {
        var notNullMaterials = _materials.Where(m => m is not null);
        var distinctMaterials = notNullMaterials.Distinct();
        Assert(notNullMaterials.Count() == distinctMaterials.Count(),
            "There is a duplicate in the materials.");
        GeneratePrefabs(_materials, _materialsTransform);
    }

    // Generate prefabs for given list, and parents them to target
    private void GeneratePrefabs(List<ShopItem> items, Transform target)
    {
        // NOTE (Chris): Hard-coded 3 value 
        // since we have only 3 coins right now.
        int[] itemCosts = new int[3];

        foreach (ShopItem item in items)
        {
            Assert(item.SellableItem != null, "Invalid item found in " +
            "shop prefab instantation.");

            UIShopItem uiItem = Instantiate(_itemPrefab, target);
            foreach (var pair in item.SellableItem.CurrencyCost.costs)
            {
                // TODO (Chris): Find a way to optimize this. It is very
                // hard-coded.

                if (pair == null || pair.First == null) 
                {
                    continue;
                }
                if (pair.First.Equals(_bronzeCoin))
                {
                    itemCosts[0] = pair.Second;
                }
                if (pair.First.Equals(_silverCoin))
                {
                    itemCosts[1] = pair.Second;
                }
                if (pair.First.Equals(_goldCoin))
                {
                    itemCosts[2] = pair.Second;
                }

                uiItem.Initialize(item.Item.Sprite, itemCosts[0],
                    itemCosts[1], itemCosts[2]);
            }

        }
    }
}