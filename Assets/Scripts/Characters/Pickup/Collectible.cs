using UnityEngine;
using UnityEngine.Pool;

using static DebugUtils;

[RequireComponent(typeof(SpriteRenderer))]
public class Collectible : MonoBehaviour
{
    [field: SerializeField]
    public ItemBase Item { get; private set; }

    [field: SerializeField]
    public uint Quantity { get; private set; }

    public Vector3 OriginalPosition { get; private set; }

    private SpriteRenderer _spriteRenderer;
    private IObjectPool<Collectible> _ownerPool = null;

    public void Initialize(IObjectPool<Collectible> pool, 
        ItemBase item, uint quantity, Vector3 position)
    {
        Item = item;
        Quantity = quantity;
        _spriteRenderer.sprite = item.Sprite;
        _ownerPool = pool;

        transform.position = position;
    }

    public void AttemptReleaseToPool()
    {
        if (_ownerPool != null)
        {
            _ownerPool.Release(this);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
    }

    private void Start()
    {
        OriginalPosition = transform.position;
        // Check if the current `Item` can decay and whether
        // it is currently enabled
        if (Item is IItemDecay decay && !decay.Disable)
        {
            float duration = 0f;

            switch (decay.DurationType)
            {
                case IItemDecay.DecayDuration.Fixed:
                    duration = decay.FixedDuration;
                    break;

                case IItemDecay.DecayDuration.Random:
                    duration = Random.Range(decay.MinDuration,
                        decay.MaxDuration);
                    break;

                default:
                    Fatal("Unhandled `DecayDuration` type");
                    break;
            }

            ItemBase target = decay.Target;

            // Execute the actual decay after the
            // `duration` has past
            this.DelayExecute(() =>
            {
                // Change the collectible item
                Item = target;

                // Change the `Sprite` on the `SpriteRenderer`
                _spriteRenderer.sprite = target.Sprite;
            }, duration);
        }
    }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
