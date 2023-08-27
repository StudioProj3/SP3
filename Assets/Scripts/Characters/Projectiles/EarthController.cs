using UnityEngine;

public class EarthController :
    ProjectileControllerBase
{
    [SerializeField]
    private LayerMask targetLayer;

    private StatusEffectBase _statusEffect;
    private Transform _source;
    private SpriteRenderer _spriteRenderer;

    public void Init(Vector3 direction, Damage damage, 
        StatusEffectBase statusEffect, 
        Transform source, Sprite sprite = null)
    {
        Init(direction, damage);

        gameObject.SetActive(true);

        _source = source;
        _statusEffect = statusEffect;

        if (sprite && _spriteRenderer.sprite != sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        float angle = -Mathf.Atan2(direction.z, direction.x) *
            Mathf.Rad2Deg;

        transform.rotation = Quaternion.
            Euler(sprite ? 90f : 0f, angle, 0f);

    }

    private void Awake()
    {
       
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _currentLifetime -= Time.deltaTime;

        if (_currentLifetime < 0f)
        {
            RemoveProjectile(_source);
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
}
