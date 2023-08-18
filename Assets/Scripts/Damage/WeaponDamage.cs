using System;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{

    public static event Action<IEffectable, Vector3> OnWeaponHit;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IEffectable>(out var effectable) && collider.CompareTag("Enemy"))
        {
            OnWeaponHit(effectable, collider.transform.position);
        }
    }
}
