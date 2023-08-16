using UnityEngine;

public interface ISwordUser : IEffectable
{
    Animator WeaponAnimator { get; } 

    SpriteRenderer CurrentWeaponRenderer { get; }

}
