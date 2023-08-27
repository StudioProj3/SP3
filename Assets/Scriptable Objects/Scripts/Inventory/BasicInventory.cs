using UnityEngine;

[CreateAssetMenu(fileName = "Inventory",
    menuName = "Scriptable Objects/Inventory/BasicInventory")]
public class BasicInventory :
    InventoryBase, ISavable
{
    [field: SerializeField]
    public override uint MaxNumSlots { get; protected set; }

    [field: SerializeField]
    public override uint MaxPerSlot { get; protected set; }

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
    protected override void SaveInventory()
    {
        SaveManager.Instance.Save(SaveID);
    }
}
