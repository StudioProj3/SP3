using UnityEngine;

public interface IInteractable
{
    public string InteractText { get; }
    public void Interact();
}