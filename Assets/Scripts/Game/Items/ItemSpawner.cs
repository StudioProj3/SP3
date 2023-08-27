using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

using UnityEngine.Pool;
using UnityEngine;
using Newtonsoft.Json;

public class ItemSpawner : MonoBehaviour, ISavable
{
    [Serializable]
    private struct StarterItemEntry
    {
        public ItemBase item;
        public uint quantity;
        public Vector3 position;
    }

    [Serializable]
    private struct ItemSaveEntry
    {
        public string itemJson;
        public uint quantity;
        public SerializableVector3 position;
    }

    [Serializable]
    private class ItemWrapper 
    {
        public ItemBase item; 

        public ItemWrapper(ItemBase item)
        {
            this.item = item;
        }
    }

    [HorizontalDivider]

    [SerializeField]
    private Collectible _droppedItemPrefab;

    [SerializeField] 
    private List<StarterItemEntry> _starterItems;

    [field: HorizontalDivider]
    [field: Header("Save Parameters")]

    [field: SerializeField]
    public bool EnableSave { get; protected set; } = true;

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public string SaveID { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public ISerializable.SerializeFormat Format
        { get; protected set; }

    private IObjectPool<Collectible> _droppedItemPool;
    private PlayerPickup _pickup;

    private void OnPickupCallback(ItemBase item, uint quantity)
    {
        SaveManager.Instance.Save(SaveID);
    }

    private void Awake()
    {
        // Might need to hook later
        HookEvents();

        _droppedItemPool = new ObjectPool<Collectible>
        (
            () => Instantiate(_droppedItemPrefab, transform),
            (Collectible collectible) => collectible.gameObject.SetActive(true),
            (Collectible collectible) => collectible.gameObject.SetActive(false)
        );
    }

    private IEnumerator Start()
    {
        yield return null;

        // Spawn all the starting objects
        _starterItems.ForEach(i => 
        {
            Collectible collectible = _droppedItemPool.Get();
            collectible.Initialize(_droppedItemPool, 
                i.item, i.quantity, i.position);
        });

        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            if (player.TryGetComponent(out _pickup))
            {
                _pickup.OnPlayerPickup.AddListener(OnPickupCallback);
            }
        }
    }

    private void OnDestroy()
    {
        _pickup.OnPlayerPickup.RemoveListener(OnPickupCallback);
    }

    public Collectible SpawnObject(ItemBase item, uint quantity, Vector3 position)
    {
        Collectible collectible = _droppedItemPool.Get();
        collectible.Initialize(_droppedItemPool, item, quantity, position);
        SaveManager.Instance.Save(SaveID);
        return collectible;
    }

    public void HookEvents()
    {
        if (EnableSave)
        {
            SaveManager.Instance.Hook(SaveID, Save, Load);
        }
    }

    public string Save()
    {
        var collectibles = GetComponentsInChildren<Collectible>();
        var collectiblePairs = collectibles
            .Select(c => new ItemSaveEntry() {
                itemJson = JsonUtility.ToJson(new ItemWrapper(c.Item)),
                quantity = c.Quantity,
                position = new SerializableVector3(c.transform.position)
            })
            .ToList();

        return JsonConvert.SerializeObject(collectiblePairs);
    }

    public void Load(string data)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        var collectiblePairs = JsonConvert
            .DeserializeObject<List<ItemSaveEntry>>(data);

        collectiblePairs.ForEach(i => SpawnObject(
            JsonUtility.FromJson<ItemWrapper>(i.itemJson).item, 
            i.quantity, i.position.UnityVector));
    }
}
