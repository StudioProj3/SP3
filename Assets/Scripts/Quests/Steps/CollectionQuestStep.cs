using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

public class CollectionQuestStep : QuestStep
{
    [HorizontalDivider]
    [Header("Quest step data")]

    [SerializeField]
    private List<Pair<ItemBase, uint>> _itemsToCollectInitializer;

    private UnityAction<ItemBase, uint> _pickupHandler;

    // The second element of the pair is the current amount.
    private Dictionary<ItemBase, Pair<uint, uint>> _itemsToCollect = new();

    public void Awake()
    {
        // Reset all the current quantities.
        _itemsToCollect.Where(kv => kv.Key != null && kv.Value != null)
            .ToList()
            .ForEach(kv => kv.Value.Second = 0);

        if (_pickupHandler == null)
        {
            _pickupHandler = new UnityAction<ItemBase, uint>(OnPlayerPickupHandler);
        }

        if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerPickup pickup))
        {
            pickup.OnPlayerPickup.AddListener(_pickupHandler);
        }
    }

    private void OnValidate()
    {
        InitializeDictionary();
    }

    private void OnEnable()
    {
        InitializeDictionary();
    }

    private void InitializeDictionary()
    {
        bool CanInitialize() => 
            _itemsToCollectInitializer.Count == _itemsToCollectInitializer
            .Select(pair => pair.First)
            .Distinct()
            .Count();

        if (CanInitialize())
        {
            _itemsToCollect = _itemsToCollectInitializer
                .Where(pair => pair != null && pair.First != null)
                .ToDictionary(kv => kv.First,
                kv => new Pair<uint, uint>(kv.Second, 0));
        }
    }

    private void OnPlayerPickupHandler(ItemBase item, uint quantity)
    {
        // We do not care about any other item.
        if (!_itemsToCollect.ContainsKey(item))
        {
            return;
        }

        // We want to limit the quantity to however much we want
        // as this quantity value can used to be shown in the UI
        _itemsToCollect[item].Second = (uint)Mathf.Min(
            (int)(_itemsToCollect[item].Second + quantity),
            (int)_itemsToCollect[item].First);

        // If we have met the quantities that we want, finish the quest step
        if (!_itemsToCollect.Values.Any(pair => pair.First > pair.Second))
        {
            FinishQuestStep();
        }
    }
}