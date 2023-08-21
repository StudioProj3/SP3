using UnityEngine;

public class UIHUDPlayerStatsBook : MonoBehaviour
{
    private Animator _animator;
    private bool _isOpen = false;
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void ToggleBook()
    {
        _isOpen = !_isOpen;
        _animator.SetBool("isOpen", _isOpen);
    }
}
