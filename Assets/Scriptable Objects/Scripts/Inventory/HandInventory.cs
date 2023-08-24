using UnityEngine;

[CreateAssetMenu(fileName = "HandInventory",
    menuName = "Scriptable Objects/Inventory/HandInventory")]
public class HandInventory :
    InventoryBase, ISavable
{
    public override uint MaxNumSlots { get; protected set; } = 2;

    public override uint MaxPerSlot { get; protected set; } = 1;

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

        _allItems = new(_itemInitializerList);
    }

    public override void Reset()
    {
        base.Reset();
    }

    // Convenience helper function for readability
    public ItemBase LeftHand()
    {
        return GetItem(0);
    }

    // Convenience helper function for readability
    public uint LeftHandAmount()
    {
        return GetAmount(0);
    }

    // Convenience helper function for readability
    public ItemBase RightHand()
    {
        return GetItem(1);
    }

    // Convenience helper function for readability
    public uint RightHandAmount()
    {
        return GetAmount(1);
    }

    protected override void OnValidate()
    {
        MaxNumSlots = 2;
        MaxPerSlot = 1;

        base.OnValidate();
    }

    private void Awake()
    {
        OnValidate();
    }
}
