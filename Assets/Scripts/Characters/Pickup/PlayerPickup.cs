using UnityEngine;
using UnityEngine.Assertions;

public class PlayerPickup :
    CharacterPickupBase
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            bool result = other.TryGetComponent(
                out Collectible collectible);

            Assert.IsTrue(result,
                "There should be a `Collectible` " +
                "component on the gameobject");

            ItemBase item = collectible.Item;
            uint quantity = collectible.Quantity;

            // Pickup inventory when the inventory slot is empty
            if (_pickupInventory && _characterData.Inventory == null &&
                item.Name == "BasicInventory")
            {
                _characterData.Inventory =
                    (item as InventoryItem).Inventory;

                other.gameObject.SetActive(false);

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
                }
            }
            // Attempt to pickup into left and/or right hand
            else
            {

            }
        }
    }
}
