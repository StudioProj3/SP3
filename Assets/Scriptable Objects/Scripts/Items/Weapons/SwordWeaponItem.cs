using System.Collections;
using UnityEngine;
using UnityEngine.Timeline;

public class SwordWeaponItem : WeaponBase, ISwordWeapon
{
    [field: SerializeField]
    private string _animationName = "MeleeSwing";
    public string AnimationName => _animationName;

    public void OnUseEnter()
    {
        
    }

    public void OnUseExit()
    {
        
    }
}
