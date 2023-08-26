using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using static DebugUtils;
using static CoinItemBase;
using UnityEngine.PlayerLoop;

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
    private CharacterData _playerData;

    [HorizontalDivider]
    [Header("Misc")]

    [SerializeField]
    private UIShopDescriptionPanel _descriptionPanel;

    [SerializeField]
    private Transform _scrollRectTransform;

    private Vector3 _scrollRectOriginalPosition;

    private UINotification UINotification 
    {
        get 
        {
            if (_uiNotification == null)
            {
                GameObject questDisplayObject = 
                    GameObject.FindWithTag("UINotification");

                if (questDisplayObject == null)
                {
                    return null;
                }

                _ = questDisplayObject.TryGetComponent(
                    out _uiNotification);
            }
            return _uiNotification;
        }
    }

    private UINotification _uiNotification;

    private Animator _animator;

    public void ShowShop(bool shopOpen)
    {
        _animator.SetBool("shopOpen", shopOpen);
        if (shopOpen)
        {
            _scrollRectTransform.position =_scrollRectOriginalPosition;
        }
    }

    public void OnItemPurchaseAttempt(UIShopItem panel)
    {
        var wealth = GetPlayerWealth(out int handBronzeAmount,
            out int handSilverAmount,
            out int handGoldAmount);

        int[] costs = (new string[3] { panel.BronzeText.text,
            panel.SilverText.text,
            panel.GoldText.text })
            .Select(str => int.Parse(str))
            .ToArray();

        // Check if we have enough money
        for (int i = CoinType.Bronze; i < CoinType.Gold + 1; ++i)
        {
            if (wealth[i] < costs[i])
            {
                Debug.Log("Not enough money.");
                panel.Shake();
                return;
            }
        }

        var item = panel.ShopItem.Item;
        uint handInventoryStackSize = _playerData.HandInventory.MaxPerSlot;

        // TODO (Chris): Do we need to try adding to the hand?
        if (_playerData.Inventory != null && _playerData.Inventory.Add(item, 1))
        {
            int[] handAmounts = new int[3] 
                { handBronzeAmount, handSilverAmount, handGoldAmount };
            ItemBase[] coinItems = new ItemBase[3]
            {
                _bronzeCoin, _silverCoin, _goldCoin
            };

            // First, we want to use the hand money first
            for (int i = CoinType.Bronze; i < CoinType.Gold + 1; ++i)
            {
                if (handAmounts[i] > 0)
                {
                    costs[i] -= handAmounts[i];
                    _playerData.HandInventory.Remove(coinItems[i],
                        (uint)handAmounts[i]);
                }

                // Then we use the players inventory coins using the rest
                if (costs[i] > 0)
                {
                    _playerData.Inventory.Remove(coinItems[i],
                        (uint)costs[i]);
                }
            }

            UINotification.Collect(item.Sprite, item.Name);
        }
        else
        {
            Debug.Log("Not enough space.");
            panel.Shake();
        }
    }

    private int[] GetPlayerWealth(out int handBronzeAmount, out int handSilverAmount, out int handGoldAmount)
    {
        int[] wealth = new int[3];

        if (_playerData.Inventory != null)
        {
            wealth[CoinType.Bronze] += _playerData.Inventory.GetAmount(_bronzeCoin);
            wealth[CoinType.Silver] += _playerData.Inventory.GetAmount(_silverCoin);
            wealth[CoinType.Gold]   += _playerData.Inventory.GetAmount(_goldCoin);
        }

        handBronzeAmount = _playerData.HandInventory.GetAmount(_bronzeCoin);
        wealth[CoinType.Bronze] += handBronzeAmount;

        handSilverAmount = _playerData.HandInventory.GetAmount(_silverCoin);
        wealth[CoinType.Silver] += handSilverAmount;

        handGoldAmount = _playerData.HandInventory.GetAmount(_goldCoin);
        wealth[CoinType.Gold] += handGoldAmount;

        return wealth;
    }

    // NOTE (Chris): We want to update with the UIShopItem since
    // we already have determined the order of the costs, since
    // it doesn't matter in the costs array in ISellable CurrencyCost
    private void UpdateDescriptionPanel(UIShopItem item, bool mouseOver)
    {
        if (mouseOver)
        {
            _descriptionPanel.UpdateHoveredShopItem(item);
        }
        _animator.SetBool("itemHovered", mouseOver);
    }

    private void Start()
    {
        var notNullMaterials = _materials.Where(m => m.SellableItem is object);
        var distinctMaterials = notNullMaterials.Distinct();
        Assert(notNullMaterials.Count() == distinctMaterials.Count(),
            "There is a duplicate in the materials.");
        // Log("Not null materials: " + notNullMaterials.Count());
        // Log("Distinct materials: " + distinctMaterials.Count());
        GeneratePrefabs(_materials, _materialsTransform);
    }

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();

        _scrollRectOriginalPosition = _scrollRectTransform.position;
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

            uiItem.Initialize(item, itemCosts[CoinType.Bronze],
                itemCosts[CoinType.Silver], itemCosts[CoinType.Gold],
                UpdateDescriptionPanel, OnItemPurchaseAttempt);
        }
    }
}
