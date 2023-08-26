using UnityEngine;

public abstract class ProjectileControllerBase :
    MonoBehaviour
{
    [HorizontalDivider]
    [Header("Projectile Parameters")]

    [SerializeField]
    [Range(0f, 10f)]
    protected float _lifetime;

    protected float _currentLifetime;
    protected Vector3 _direction;
    protected Damage _damage;

    private void RemoveProjectile(Transform source = null)
    {
        gameObject.SetActive(false);

        if (source)
        {
            transform.SetParent(source);
        }
    }
}
