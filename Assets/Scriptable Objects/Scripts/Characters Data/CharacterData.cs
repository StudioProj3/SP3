using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData",
    menuName = "Scriptable Objects/Character Data")]
public class CharacterData : ScriptableObject,
    INameable, ISavable
{
    [field: HorizontalDivider]
    [field: Header("Basic Parameters")]

    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public bool IsDead { get; set; }

    [field: HorizontalDivider]
    [field: Header("Inventory Parameters")]

    [field: SerializeField]
    public InventoryBase Inventory { get; set; }

    [field: SerializeField]
    public HandInventory HandInventory { get; set; }

    [field: HorizontalDivider]
    [field: Header("Stats Parameters")]

    [field: SerializeField]
    public Stats CharacterStats { get; set; }

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
    }

    public virtual void Reset()
    {
        Inventory = null;

        if (HandInventory != null)
        {
            HandInventory.Reset();
            HandInventory.HookEvents();
        }

        IsDead = false;
    }

    protected virtual void OnValidate()
    {
#if UNITY_EDITOR

        Name = name;
        UnityEditor.EditorUtility.SetDirty(this);

#endif
    }
}
