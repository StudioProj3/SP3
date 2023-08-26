using UnityEngine;

public class UIHUDSettingsPage : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ToggleSettings()
    {
        _isOpen = !_isOpen;
        _animator.SetBool("isOpen", _isOpen);
    }
}
