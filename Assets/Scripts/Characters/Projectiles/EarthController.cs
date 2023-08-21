using UnityEngine;

public class EarthController : MonoBehaviour
{

    [field: SerializeField]
    private float _lifetime;

    [field: SerializeField]
    private LayerMask targetLayer;

    private float _currentLifetime;
    private Vector3 _direction;
    private Damage _damage;
    private StatusEffectBase _statusEffect;
    private Transform _source;
    private SpriteRenderer _spriteRenderer;

    public void Init(Vector3 direction, Damage damage, 
        StatusEffectBase statusEffect, 
        Transform source, Sprite sprite)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _damage = damage;
        _source = source;
        _statusEffect = statusEffect;

        _currentLifetime = _lifetime;

        if (_spriteRenderer.sprite != sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(90, angle, 0);
    }
    public void Init(Vector3 direction, Damage damage, 
        StatusEffectBase statusEffect, 
        Transform source)
    {
        gameObject.SetActive(true);
        _direction = direction;
        _damage = damage;
        _source = source;
        _statusEffect = statusEffect;

        _currentLifetime = _lifetime;

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void Awake()
    {
        gameObject.SetActive(false);  
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
        }
    }

    private void RemoveProjectile()
    {
        gameObject.SetActive(false);
        transform.SetParent(_source);
    }
}