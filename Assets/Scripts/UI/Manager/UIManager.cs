using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [HorizontalDivider]
    [Header("Black Bars Static Data")]
    [SerializeField] 
    private float _blackBarSpeed = 1.0f;

    [HorizontalDivider]
    [Header("HUD UI Static Data")]
    [SerializeField]
    private float _hudAnimationSpeed = 1.0f;

    private Animator _animator;
    private Animator _hudAnimator = null;


    protected override void OnStart()
    {
        _animator = GetComponent<Animator>();
        _animator.SetFloat("displacementSpeed", _blackBarSpeed);

        // NOTE (Chris): This code is assuming that the HUD object is persistent
        GameObject hud = GameObject.FindWithTag("HUD");
        if (hud != null)
        {
            if (_hudAnimator.TryGetComponent(out Animator animator))
            {
                _hudAnimator = animator;
                _hudAnimator.SetFloat("speedMultiplier", _hudAnimationSpeed);
            }
        }
    }

    public void ShowBars(bool showBars)
    {
        _animator.SetBool("showBars", showBars);
    }

    public void ShowHUD(bool showHUD)
    {
        if (_hudAnimator != null)
        {
            _hudAnimator.SetBool("hudShow", showHUD);
        }
    }
}
