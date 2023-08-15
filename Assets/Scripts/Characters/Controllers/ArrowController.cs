using System.Collections.Generic;
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

    public void Init(Vector3 direction, PhysicalDamage phyDamage, PlayerController playerController)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _phyDamage = phyDamage;
        _playerController = playerController;

        _rigidbody.velocity = _direction * _speed;
        _currentLifetime = _lifetime;
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameObject.SetActive(false);
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        _currentLifetime -= Time.deltaTime;
        if(_currentLifetime < 0)
            RemoveProjectile();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            _playerController.TakeDamage(_phyDamage);
            RemoveProjectile();
        }
        else if (col.gameObject.tag == "Scene Object")
        {
            RemoveProjectile();
        }

    }

    private void RemoveProjectile()
    {
        gameObject.SetActive(false);
    }
}
