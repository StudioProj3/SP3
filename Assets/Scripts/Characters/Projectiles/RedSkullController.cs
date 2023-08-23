using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSkullController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _lifetime;

    [SerializeField]
    private float _knockback;

    private float _currentLifetime;

    private Vector3 _direction;
    private Damage _damage;
    private PlayerController _playerController;
    private Transform _source;

    private Rigidbody _rigidbody;

    public void Init(Vector3 direction, Damage damage,
        PlayerController playerController, Transform source)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _damage = damage;
        _playerController = playerController;
        _source = source;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = _direction * _speed;
        _currentLifetime = _lifetime;

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        //transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Awake()
    {
    }

    private void Update()
    {
        _currentLifetime -= Time.deltaTime;

        if (_currentLifetime < 0f)
        {
            RemoveProjectile();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Vector3 knockbackForce = _direction * _knockback;

            _playerController.TakeDamage(_damage,knockbackForce);
            RemoveProjectile();
        }
        
    }

    private void RemoveProjectile()
    {
        gameObject.SetActive(false);
        transform.SetParent(_source);
    }
}
