using UnityEngine;

public abstract class InteractableBase : MonoBehaviour
{
    [HorizontalDivider]
    [Header("Interactable Parameters")]

    [SerializeField]
    protected string _interactText;

    protected abstract void Interact();
}
