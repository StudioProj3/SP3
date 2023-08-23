using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [HorizontalDivider]
    [Header("Black Bars Static Data")]
    [SerializeField] 
    private float _blackBarSpeed = 1.0f;

    private Animator _animator;

    protected override void OnStart()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("displacementSpeed", _blackBarSpeed);
    }

    public void ShowBars(bool showBars)
    {
        _animator.SetBool("showBars", showBars);
    }
}
