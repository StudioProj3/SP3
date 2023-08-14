using UnityEngine;

public class Collectible : MonoBehaviour
{
    [field: SerializeField]
    public ItemBase Item { get; private set; }

    [field: SerializeField]
    public uint Quantity { get; private set; }
}
