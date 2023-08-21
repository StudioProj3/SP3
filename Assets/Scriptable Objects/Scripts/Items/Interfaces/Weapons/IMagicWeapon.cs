using UnityEngine;

public interface IMagicWeapon : 
    IBeginUseHandler, IUseHandler, IEndUseHandler
{
    string AnimationName { get; }

    ArrowItem Projectile { get; }

    float SanityCost { get; }

    void Shoot(ArrowController projectile, Vector3 direction, Transform source);
}