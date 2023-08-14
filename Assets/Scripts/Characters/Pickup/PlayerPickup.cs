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

            other.gameObject.SetActive(false);
        }
    }
}
