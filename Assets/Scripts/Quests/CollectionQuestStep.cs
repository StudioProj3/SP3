using UnityEngine;
using UnityEngine.Events;

public class CollectionQuestStep : QuestStep
{
    [field: SerializeField]
    public ItemBase ItemBase { get; private set; }

    [field: SerializeField]
    public uint Quantity { get; private set; } 

    private UnityAction<ItemBase, uint> _pickupHandler;
    private uint _currentQuantity; 

    public void Awake()
    {
        _currentQuantity = 0;

        if (_pickupHandler == null)
        {
            _pickupHandler = new UnityAction<ItemBase, uint>(OnPlayerPickupHandler);
        }

        if (GameObject.FindWithTag("Player").TryGetComponent(out PlayerPickup pickup))
        {
            pickup.OnPlayerPickup.AddListener(_pickupHandler);
        }
    }

    private void OnPlayerPickupHandler(ItemBase item, uint quantity)
    {
        // We do not care about any other item.
        if (item != ItemBase) 
        {
            return;
        }

        // We want to limit the quantity to however much we want
        // as this quantity value can used to be shown in the UI
        _currentQuantity = (uint)Mathf.Min((int)(_currentQuantity + quantity),
            (int)Quantity);

        // If we have met the quantity that we want, finish the quest step
        if (_currentQuantity == Quantity)
        {
            FinishQuestStep();
        }
    }
}