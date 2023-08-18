using System.Runtime.CompilerServices;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [field: SerializeField]
    private float _speed;

    [field: SerializeField]
    private float _lifetime;

    [field: SerializeField]
    private LayerMask targetLayer;

    private float _currentLifetime;
    private Vector3 _direction;
    private PhysicalDamage _phyDamage;
    private Transform _source;
    private Rigidbody _rigidbody;

    public void Init(Vector3 direction, PhysicalDamage phyDamage, 
        Transform source)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _phyDamage = phyDamage;
        _source = source;

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = _direction * _speed;
        _currentLifetime = _lifetime;

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Awake()
    {
        gameObject.SetActive(false);  
    }

    private void Update()
    {
        _currentLifetime -= Time.deltaTime;

        if (_currentLifetime < 0f)
        {
            RemoveProjectile();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IEffectable>(out var effectable) &&
            (targetLayer.value & 1 << collider.gameObject.layer) != 0)
        {
            Vector3 knockbackForce = (collider.transform.position - transform.position).normalized * 5;
            effectable.TakeDamage(_phyDamage, knockbackForce);
            RemoveProjectile();
        }
        else if (collider.gameObject.CompareTag("Scene Object"))
        {
            RemoveProjectile();
        }
    }

    private void RemoveProjectile()
    {
        gameObject.SetActive(false);
        transform.SetParent(_source);
    }
}