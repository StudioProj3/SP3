using UnityEngine;

using static DebugUtils;

[RequireComponent(typeof(SpriteRenderer))]
public class Collectible : MonoBehaviour
{
    [field: SerializeField]
    public ItemBase Item { get; private set; }

    [field: SerializeField]
    public uint Quantity { get; private set; }

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
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
