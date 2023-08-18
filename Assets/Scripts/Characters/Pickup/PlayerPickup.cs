using UnityEngine;

using static DebugUtils;

public class PlayerPickup :
    CharacterPickupBase
{
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

            // Pickup inventory when the inventory slot is empty
            if (_pickupInventory && _characterData.Inventory == null &&
                item.Name == "BasicInventory")
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

                // Do not attempt to trigger notification if the
                // UI objects are not found
                if (_notification)
                {
                    _notification.Collect(sprite, name);
                }

                return;
            }

            // Attempt to pickup into inventory
            if (_characterData.Inventory)
            {
                bool tryPickup = _characterData.Inventory.Add(item, quantity);

                // Only if the character can successfully
                // pick the items up
                if (tryPickup)
                {
                    other.gameObject.SetActive(false);

                    // Do not attempt to trigger notification if the
                    // UI objects are not found
                    if (_notification)
                    {
                        _notification.Collect(sprite, name);
                    }
                }
            }
            // Attempt to pickup into left and/or right hand
            else
            {

            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }
}
