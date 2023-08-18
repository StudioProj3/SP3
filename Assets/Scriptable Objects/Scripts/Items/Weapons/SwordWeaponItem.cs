using UnityEngine;

[CreateAssetMenu(fileName = "Sword",
    menuName = "Scriptable Objects/Items/Weapons/Sword")]
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
