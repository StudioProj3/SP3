using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
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

    private Rigidbody _rigidbody;

    private Animator _spawnLaserAnimator;
    private Animator _laserAnimator;

    private bool collide;

    public void Init(Vector3 direction, Damage damage,
        PlayerController playerController)
    {

        gameObject.SetActive(true);

        _direction = direction;
        _damage = damage;
        _playerController = playerController;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = _direction * _speed;
        _currentLifetime = _lifetime;

        _spawnLaserAnimator = transform.GetChild(0).GetComponent<Animator>();
        _laserAnimator = transform.GetChild(1).GetComponent<Animator>();

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        collide = false;

        //transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void SetAnimatorBool(string boolToSet, bool state)
    {
        _spawnLaserAnimator.SetBool(boolToSet, state);
        _laserAnimator.SetBool(boolToSet, state);
    }

    public void SetCollide(bool state)
    {
        collide = state;
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

    private void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player" && collide)
        {
            Vector3 knockbackForce = _direction * _knockback;

            _playerController.TakeDamage(_damage, knockbackForce);
        }

    }

    private void RemoveProjectile()
    {
        gameObject.SetActive(false);
    }
}
