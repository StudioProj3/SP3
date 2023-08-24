using UnityEngine;

public abstract class CharacterPickupBase :
    MonoBehaviour
{
    [HorizontalDivider]
    [Header("Character Data")]

    [SerializeField]
    protected CharacterData _characterData;

    [HorizontalDivider]
    [Header("Allowed pickups")]

    [SerializeField]
    protected bool _pickupInventory = false;

    protected UINotification _notification;

    protected abstract void OnTriggerEnter(Collider other);

    protected virtual void Update()
    {
        if (_notification)
        {
            return;
        }

        GameObject notifUI = GameObject.FindWithTag("UINotification");

        if (notifUI)
        {
            _notification = notifUI.GetComponent<UINotification>();
        }
    }
}
