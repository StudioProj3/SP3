using System;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDamage : MonoBehaviour
{

    public static event Action<IEffectable> OnWeaponHit;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IEffectable>(out var effectable) && collider.CompareTag("Enemy"))
        {
            OnWeaponHit(effectable);
        }
    }
}
