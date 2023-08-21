using UnityEngine;

public interface IInteractable
{
    public string interactText { get; }
    public void Interact();
    public bool CheckPlayer();
}