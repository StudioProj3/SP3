using UnityEngine;

public class LaserController :
    ProjectileControllerBase
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _knockback;

    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private Animator _spawnLaserAnimator;
    private Animator _laserAnimator;
    private bool _collide;

    public void Init(Vector3 direction, Damage damage,
        PlayerController playerController)
    {
        Init(direction, damage);

        gameObject.SetActive(true);

        _playerController = playerController;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = _direction * _speed;

        _spawnLaserAnimator = transform.GetChild(0).
            GetComponent<Animator>();
        _laserAnimator = transform.GetChild(1).
            GetComponent<Animator>();

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        _collide = false;
    }

    public void SetAnimatorBool(string boolToSet, bool state)
    {
        _spawnLaserAnimator.SetBool(boolToSet, state);
        _laserAnimator.SetBool(boolToSet, state);
    }

    public void SetCollide(bool state)
    {
        _collide = state;
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
        if (col.CompareTag("Player") && _collide)
        {
            Vector3 knockbackForce = _direction * _knockback;

            _playerController.TakeDamage(_damage, knockbackForce);
        }
    }
}
