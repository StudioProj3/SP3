using UnityEngine;

[CreateAssetMenu(fileName = "BasicRecipeInventory",
    menuName = "Scriptable Objects/Inventory/Crafting/" +
    "BasicRecipeInventory")]
public class BasicRecipeInventory :
    InventoryBase, ISavable
{
    public override uint MaxNumSlots { get; protected set; } = 4;

    public override uint MaxPerSlot { get; protected set; } = 10;

    [field: HorizontalDivider]
    [field: Header("Save Parameters")]

    [field: SerializeField]
    public bool EnableSave { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public string SaveID { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public ISerializable.SerializeFormat Format
        { get; protected set; }

    public void HookEvents()
    {
        if (EnableSave)
        {
            SaveManager.Instance.Hook(SaveID, Save, Load);
        }
    }

    public string Save()
    {
        ISerializable serializable = this;
        return serializable.Serialize();
    }

    public void Load(string data)
    {
        IDeserializable deserializable = this;
        deserializable.Deserialize(data);

        for (int i = 0; i < _itemInitializerList.Count; ++i)
        {
            _allItems[i] = _itemInitializerList[i].Key ?
                _itemInitializerList[i] : null;
        }
    }

    public override void Reset()
    {
        base.Reset();
    }

    // Convenience helper function for readability
    public ItemBase Row1Col1()
    {
        return GetItem(0);
    }

    // Convenience helper function for readability
    public uint Row1Col1Amount()
    {
        return GetAmount(0);
    }

    // Convenience helper function for readability
    public ItemBase Row1Col2()
    {
        return GetItem(1);
    }

    // Convenience helper function for readability
    public uint Row1Col2Amount()
    {
        return GetAmount(1);
    }

    // Convenience helper function for readability
    public ItemBase Row2Col1()
    {
        return GetItem(2);
    }

    // Convenience helper function for readability
    public uint Row2Col1Amount()
    {
        return GetAmount(2);
    }

    // Convenience helper function for readability
    public ItemBase Row2Col2()
    {
        return GetItem(3);
    }

    // Convenience helper function for readability
    public uint Row2Col2Amount()
    {
        return GetAmount(3);
    }

    protected override void OnValidate()
    {
        MaxNumSlots = 4;
        MaxPerSlot = 10;

        base.OnValidate();
    }

    private void Awake()
    {
        OnValidate();
    }
}
