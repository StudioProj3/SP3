using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _lifetime;

    private float _currentLifetime;

    private Vector3 _direction;
    private PhysicalDamage _phyDamage;
    private PlayerController _playerController;

    private Rigidbody _rigidbody;

    public void Init(Vector3 direction, PhysicalDamage phyDamage,
        PlayerController playerController)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _phyDamage = phyDamage;
        _playerController = playerController;

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

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        { 
            Vector3 knockbackForce = (col.transform.position - transform.position).normalized * 5;
            _playerController.TakeDamage(_phyDamage, knockbackForce);
            RemoveProjectile();
        }
        else if (col.gameObject.CompareTag("Scene Object"))
        {
            RemoveProjectile();
        }
    }

    private void RemoveProjectile()
    {
        gameObject.SetActive(false);
    }
}
