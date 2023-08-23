using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    private TrueDamage _lavaDamage;

    private void Awake()
    {
        _lavaDamage = TrueDamage.Create(1);
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.TryGetComponent<IEffectable>(out var effectable) &&
            col.CompareTag("Player"))
        {
            effectable.TakeDamage(_lavaDamage, new Vector3(0,0,0));
        }
    }
}
