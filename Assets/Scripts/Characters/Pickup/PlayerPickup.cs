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

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            // _characterData.CharacterStats.GetStat("Health").AddModifier(Modifier.Plus(20, 0));

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

                    SaveManager.Instance.Save(_characterData.SaveID);

                    collectible.AttemptReleaseToPool();

                    TryNotificationCollect(_notification, sprite, name,
                        quantity);
                }

                return;
            }


            // Attempt to pickup into hand inventory
            bool tryHandInventory = _characterData.HandInventory.
                Add(item, quantity);

            if (tryHandInventory)
            {
                SaveManager.Instance.Save(_characterData.
                    HandInventory.SaveID);

                collectible.AttemptReleaseToPool();

                TryNotificationCollect(_notification, sprite, name,
                    quantity);
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
                    SaveManager.Instance.Save(
                        (_characterData.Inventory as BasicInventory).
                        SaveID);

                    collectible.AttemptReleaseToPool();
                    OnPlayerPickup?.Invoke(item, quantity);

                    TryNotificationCollect(_notification, sprite, name,
                        quantity);

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
        Sprite sprite, string name, uint quantity)
    {
        // Do not attempt to trigger notification if the
        // UI objects are not found
        if (notification == null)
        {
            return;
        }

        notification.Collect(sprite, name, quantity);
    }
}
