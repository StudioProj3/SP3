using UnityEngine;

public interface IBowWeapon : 
    IBeginUseHandler, IUseHandler, IEndUseHandler
{
    string AnimationName { get; }

    GameObject Projectile {get;}
}