using UnityEngine;

[CreateAssetMenu(fileName = "NormalRecipeInventory",
    menuName = "Scriptable Objects/Inventory/Crafting/" +
    "NormalRecipeInventory")]
public class NormalRecipeInventory :
    InventoryBase, ISavable
{
    public override uint MaxNumSlots { get; protected set; } = 9;

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
    public ItemBase Row1Col3()
    {
        return GetItem(2);
    }

    // Convenience helper function for readability
    public uint Row1Col3Amount()
    {
        return GetAmount(2);
    }

    // Convenience helper function for readability
    public ItemBase Row2Col1()
    {
        return GetItem(3);
    }

    // Convenience helper function for readability
    public uint Row2Col1Amount()
    {
        return GetAmount(3);
    }

    // Convenience helper function for readability
    public ItemBase Row2Col2()
    {
        return GetItem(4);
    }

    // Convenience helper function for readability
    public uint Row2Col2Amount()
    {
        return GetAmount(4);
    }

    // Convenience helper function for readability
    public ItemBase Row2Col3()
    {
        return GetItem(5);
    }

    // Convenience helper function for readability
    public uint Row2Col3Amount()
    {
        return GetAmount(5);
    }

    // Convenience helper function for readability
    public ItemBase Row3Col1()
    {
        return GetItem(6);
    }

    // Convenience helper function for readability
    public uint Row3Col1Amount()
    {
        return GetAmount(6);
    }

    // Convenience helper function for readability
    public ItemBase Row3Col2()
    {
        return GetItem(7);
    }

    // Convenience helper function for readability
    public uint Row3Col2Amount()
    {
        return GetAmount(7);
    }

    // Convenience helper function for readability
    public ItemBase Row3Col3()
    {
        return GetItem(8);
    }

    // Convenience helper function for readability
    public uint Row3Col3Amount()
    {
        return GetAmount(8);
    }

    protected override void OnValidate()

    {
        MaxNumSlots = 9;
        MaxPerSlot = 10;

        base.OnValidate();
    }

    private void Awake()
    {
        OnValidate();
    }
}
