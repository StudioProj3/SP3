using UnityEngine;

public class MissleController : MonoBehaviour
{
    [field: SerializeField]
    private float _speed;

    [field: SerializeField]
    private float _lifetime;

    [field: SerializeField]
    private LayerMask targetLayer;

    [SerializeField]
    private ArrowItem _arrowInfo;

    private float _currentLifetime;
    private Vector3 _direction;
    private Damage _damage;
    private StatusEffectBase _statusEffect;
    private Transform _source;
    private Rigidbody _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private GameObject _player;

    public void Init(Vector3 direction, Damage damage,
        StatusEffectBase statusEffect,
        Transform source, Sprite sprite)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _damage = damage;
        _source = source;
        _statusEffect = statusEffect;

        _rigidbody.velocity = _direction * _speed;

        _currentLifetime = _lifetime;

        if (_spriteRenderer.sprite != sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        _rigidbody.AddForce(_direction.normalized * 0.25f,
            ForceMode.Impulse);
    }

    public void Init(Vector3 direction, Damage damage,
        StatusEffectBase statusEffect,
        Transform source, GameObject player)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _damage = damage;
        _source = source;
        _statusEffect = statusEffect;
        _player = player;

        //_rigidbody.velocity = _direction * _speed;

        _currentLifetime = _lifetime;

        _rigidbody.AddForce(_direction.normalized * 0.25f,
            ForceMode.Impulse);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _currentLifetime -= Time.deltaTime;

        if (_currentLifetime < 0f)
        {
            RemoveProjectile();
        }
    }

    private void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            _direction = _player.transform.position -
                transform.position;

            _rigidbody.AddForce(_direction.normalized * 0.15f,
                ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<IEffectable>(out var effectable) &&
            (targetLayer.value & 1 << collider.gameObject.layer) != 0)
        {
            Vector3 knockbackForce = _direction * 1.5f;
            effectable.TakeDamage(_damage, knockbackForce);

            if (_statusEffect)
            {
                effectable.ApplyEffect(_statusEffect.Clone());
            }

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
