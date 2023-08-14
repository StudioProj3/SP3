using UnityEngine;

public abstract class CharacterPickupBase :
    MonoBehaviour
{
    [HorizontalDivider]
    [Header("Character Data")]

    [SerializeField]
    protected CharacterDataBase _characterData;

    [HorizontalDivider]
    [Header("Allowed pickups")]

    protected bool pickupInventory = false;

    protected abstract void OnTriggerEnter(Collider other);
}
