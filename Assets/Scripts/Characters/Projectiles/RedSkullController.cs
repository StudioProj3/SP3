using UnityEngine;

public class RedSkullController :
    ProjectileControllerBase
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _knockback;

    private PlayerController _playerController;
    private Transform _source;
    private Rigidbody _rigidbody;

    public void Init(Vector3 direction, Damage damage,
        PlayerController playerController, Transform source)
    {
        Init(direction, damage);

        gameObject.SetActive(true);

        _playerController = playerController;
        _source = source;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = _direction * _speed;

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        //transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Update()
    {
        _currentLifetime -= Time.deltaTime;

        if (_currentLifetime < 0f)
        {
            RemoveProjectile(_source);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Vector3 knockbackForce = _direction * _knockback;

            _playerController.TakeDamage(_damage,knockbackForce);
            RemoveProjectile(_source);
        }
    }
}
