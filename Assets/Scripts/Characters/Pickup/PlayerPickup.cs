using UnityEngine;

public class PlayerPickup :
    CharacterPickupBase
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inventory Bag"))
        {
            Debug.Log("A");
        }
    }
}
