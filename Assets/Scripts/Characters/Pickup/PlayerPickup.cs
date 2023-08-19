using System;
using UnityEngine;
using UnityEngine.Events;
using static DebugUtils;

public class PlayerPickup :
    CharacterPickupBase
{
    [field: HorizontalDivider]
    [field: Header("Events")]
    [field: SerializeField]
    public UnityEvent<ItemBase, uint> OnPlayerPickup { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        OnPlayerPickup = new UnityEvent<ItemBase, uint>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            bool result = other.TryGetComponent(
                out Collectible collectible);

            Assert(result, "There should be a `Collectible` " +
                "component on the gameobject");

            ItemBase item = collectible.Item;
            Sprite sprite = item.Sprite;
            string name = item.Name;
            uint quantity = collectible.Quantity;

            // Handle inventory item
            if (item is InventoryItem)
            {
                if (_pickupInventory && _characterData.Inventory == null)
                {
                    _characterData.Inventory =
                        (item as InventoryItem).Inventory;

                    // TODO (Cheng Jun): This should use the player's
                    // local save and not reset on collect once the save
                    // system is ready. For now it resets on collect as
                    // the scriptable object saves its state which is
                    // undesirable when in development
                    _characterData.Inventory.Reset();

                    other.gameObject.SetActive(false);

                    TryNotificationCollect(_notification, sprite, name);
                }

                return;
            }

            // Attempt to pickup into hand inventory
            bool tryHandInventory = _characterData.HandInventory.
                Add(item, quantity);

            if (tryHandInventory)
            {
                other.gameObject.SetActive(false);

                TryNotificationCollect(_notification, sprite, name);
                OnPlayerPickup?.Invoke(item, quantity);

                return;
            }

            // Attempt to pickup into main inventory
            if (_characterData.Inventory)
            {
                bool tryInventory = _characterData.Inventory.
                    Add(item, quantity);

                // Only if the character can
                // successfully pick the items up
                if (tryInventory)
                {
                    other.gameObject.SetActive(false);
                    OnPlayerPickup?.Invoke(item, quantity);

                    TryNotificationCollect(_notification, sprite, name);

                    return;
                }
            }

            if (_notification)
            {
                _notification.Error("All inventory slots full");
            }
        }
    }


    private void TryNotificationCollect(UINotification notification,
        Sprite sprite, string name)
    {
        // Do not attempt to trigger notification if the
        // UI objects are not found
        if (notification == null)
        {
            return;
        }

        notification.Collect(sprite, name);
    }
}
