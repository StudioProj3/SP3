using UnityEngine;

public interface IBowWeapon : 
    IBeginUseHandler, IUseHandler, IEndUseHandler
{
    string AnimationName { get; }

    ItemBase Projectile {get;}

    void Shoot(ArrowController projectile, Vector3 direction,Transform source);
}