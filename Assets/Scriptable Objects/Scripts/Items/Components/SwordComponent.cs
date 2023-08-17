using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SwordComponent",
    menuName = "Scriptable Objects/Item Components/SwordComponent")]
public class SwordComponent : WeaponComponentBase
{
    public void Swing(Animator animator)
    {
        animator.Play("MeleeSwing");
    }
}
