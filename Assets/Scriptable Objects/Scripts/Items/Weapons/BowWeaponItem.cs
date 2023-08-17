using UnityEngine;

[CreateAssetMenu(fileName = "Bow",
    menuName = "Scriptable Objects/Items/Weapons/Bow")]
public class BowWeaponItem : WeaponBase, IBowWeapon
{
    [field: SerializeField]
    private string _animationName = "Shoot";
    public string AnimationName => _animationName;

    public void OnUseEnter()
    {
        
    }

    public void OnUse()
    {
        
    }

    public void OnUseExit()
    {
        
    }
}
