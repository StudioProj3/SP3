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

    protected UINotification _notification;

    protected abstract void OnTriggerEnter(Collider other);

    protected virtual void Awake()
    {
        GameObject notifUI = GameObject.FindWithTag("UINotification");

        if (notifUI)
        {
            _notification = notifUI.GetComponent<UINotification>();
        }
    }
}
