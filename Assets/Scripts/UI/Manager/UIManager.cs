using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [HorizontalDivider]
    [Header("Black Bars Static Data")]

    [SerializeField] 
    [Range(0.0f, 100.0f)]
    private float _blackBarSpeed = 1.0f;

    [HorizontalDivider]
    [Header("HUD UI Static Data")]

    [SerializeField]
    [Range(0.0f, 100.0f)]
    private float _hudAnimationSpeed = 1.0f;

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

    public void ShowHUD(bool showHUD)
    {
        GameObject hud = GameObject.FindWithTag("HUD");
        if (hud != null)
        {
            if (hud.TryGetComponent(out Animator animator))
            {
                animator.SetFloat("speedMultiplier", _hudAnimationSpeed);
                animator.SetBool("hudShow", showHUD);
            }
        }

        // NOTE (Chris): Keep this code here in case we want to cache the HUD
        // This code is assuming that the HUD object is persistent
        // if (_hudAnimator != null)
        // {
        //     _hudAnimator.SetBool("hudShow", showHUD);
        // }
    }
}
