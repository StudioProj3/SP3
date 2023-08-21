using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;
using static CoinItemBase;

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

    [HorizontalDivider]
    [Header("Player Data")]

    [SerializeField]
    private InventoryBase _playerInventory;

    private void OnValidate()
    {

    }

    private int[] GetPlayerWealth()
    {
        int[] wealth = new int[3];
        wealth[CoinType.Bronze] = _playerInventory.GetAmount(_bronzeCoin);
        wealth[CoinType.Silver] = _playerInventory.GetAmount(_silverCoin);
        wealth[CoinType.Gold] = _playerInventory.GetAmount(_goldCoin);

        return wealth;
    }

    private void Start()
    {
        var notNullMaterials = _materials.Where(m => m.SellableItem is object);
        var distinctMaterials = notNullMaterials.Distinct();
        Assert(notNullMaterials.Count() == distinctMaterials.Count(),
            "There is a duplicate in the materials.");
        // Debug.Log("Not null materials: " + notNullMaterials.Count());
        // Debug.Log("Distinct materials: " + distinctMaterials.Count());
        GeneratePrefabs(_materials, _materialsTransform);
    }

    // Generate prefabs for given list, and parents them to target
    private void GeneratePrefabs(List<ShopItem> items, Transform target)
    {
        // NOTE (Chris): Hard-coded 3 value 
        // since we have only 3 coins right now.
        ItemBase[] coinItems = new ItemBase[3]
        {
            _bronzeCoin, _silverCoin, _goldCoin
        };

        foreach (ShopItem item in items)
        {
            int[] itemCosts = new int[3];
            Assert(item.SellableItem != null, "Invalid item found in " +
            "shop prefab instantation.");

            UIShopItem uiItem = Instantiate(_itemPrefab, target);
            foreach (var pair in item.SellableItem.CurrencyCost.costs)
            {
                if (pair == null || pair.First == null) 
                {
                    continue;
                }

                for (int i = 0; i < itemCosts.Length; ++i)
                {
                    if (pair.First.Equals(coinItems[i]))
                    {
                        itemCosts[i] = pair.Second;
                    }
                }
            }
            uiItem.Initialize(item.Item, itemCosts[CoinType.Bronze],
                itemCosts[CoinType.Silver], itemCosts[CoinType.Gold]);

        }
    }
}
