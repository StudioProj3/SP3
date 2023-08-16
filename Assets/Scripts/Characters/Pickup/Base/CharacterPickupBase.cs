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

    [SerializeField]
    protected bool _pickupInventory = false;

    protected abstract void OnTriggerEnter(Collider other);
}
