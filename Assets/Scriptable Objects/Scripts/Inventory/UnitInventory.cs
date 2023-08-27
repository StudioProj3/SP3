using UnityEngine;

[CreateAssetMenu(fileName = "UnitInventory",
    menuName = "Scriptable Objects/Inventory/UnitInventory")]
public class UnitInventory :
    InventoryBase, ISavable
{
    public override uint MaxNumSlots { get; protected set; } = 1;

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
    public ItemBase Slot()
    {
        return GetItem(0);
    }

    // Convenience helper function for readability
    public uint SlotAmount()
    {
        return GetAmount(0);
    }

    protected override void OnValidate()
    {
        MaxNumSlots = 1;
        MaxPerSlot = 10;

        base.OnValidate();
    }

    private void Awake()
    {
        OnValidate();
    }
}
