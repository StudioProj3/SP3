using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    private TrueDamage _lavaDamage;
    private float _lavaTimer;

    private void Awake()
    {
        _lavaDamage = TrueDamage.Create(2);
        _lavaTimer = 1;
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.TryGetComponent<IEffectable>(out var effectable) &&
            col.CompareTag("Player"))
        {
            _lavaTimer -= Time.deltaTime;
            if (_lavaTimer <= 0)
            {
                effectable.TakeDamage(_lavaDamage, new Vector3(0, 0, 0));
                _lavaTimer = 1;
            }
        }
    }
}
