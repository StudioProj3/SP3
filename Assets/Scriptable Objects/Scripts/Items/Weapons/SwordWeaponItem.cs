using Unity.VisualScripting;
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
        if (CanAttack)
        {
            CanAttack = false;
            
            _ = Delay.Execute(() =>
            {
                Debug.Log("CAN ATTACK AGAIN");
                CanAttack = true;
            }, WeaponStats.GetStat("AttackCooldown").Value);
        }
        
    }

    public void OnUseExit()
    {
        Debug.Log("EXITING");
    }
}
