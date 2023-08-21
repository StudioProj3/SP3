using UnityEngine;

public interface IBowWeapon : 
    IBeginUseHandler, IUseHandler, IEndUseHandler
{
    string AnimationName { get; }

    ArrowItem Projectile { get; }

    void Shoot(ArrowController projectile, Vector3 direction, Transform source);
}
