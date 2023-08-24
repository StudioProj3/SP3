using System.Collections.Generic;
using System;

using UnityEngine.Pool;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Serializable]
    private struct StarterItemEntry
    {
        public ItemBase item;
        public uint quantity;
        public Vector3 position;
    }

    [HorizontalDivider]

    [SerializeField]
    private Collectible _droppedItemPrefab;

    [SerializeField] 
    private List<StarterItemEntry> _starterItems;

    private IObjectPool<Collectible> _droppedItemPool;

    private void Awake()
    {
        _droppedItemPool = new ObjectPool<Collectible>
        (
            () => Instantiate(_droppedItemPrefab, transform),
            (Collectible collectible) => collectible.gameObject.SetActive(true),
            (Collectible collectible) => collectible.gameObject.SetActive(false)
        );

        // Spawn all the starting objects
        _starterItems.ForEach(i => 
        {
            Collectible collectible = _droppedItemPool.Get();
            collectible.Initialize(_droppedItemPool, 
                i.item, i.quantity, i.position);
        });
    }

    public void SpawnObject(ItemBase item, uint quantity, Vector3 position)
    {
        Collectible collectible = _droppedItemPool.Get();
        collectible.Initialize(_droppedItemPool, item, quantity, position);
    }
}
