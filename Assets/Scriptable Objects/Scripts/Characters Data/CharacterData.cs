using System;

using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData",
    menuName = "Scriptable Objects/Character Data")]
public class CharacterData : ScriptableObject,
    INameable, ISavable<CharacterData>
{
    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public InventoryBase Inventory { get; set; }

    [field: SerializeField]
    public HandInventory HandInventory { get; set; }

    [field: SerializeField]
    public bool IsDead { get; set; }

    [field: SerializeField]
    public Stats CharacterStats { get; set; }

    [field: HorizontalDivider]
    [field: Header("Save Parameters")]

    [field: SerializeField]
    public bool EnableSave { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("EnableSave", true, true)]
    public string SaveID { get; protected set; }

    public string Serialize()
    {
        return "";
    }

    public CharacterData Deserialize()
    {
        return new();
    }

    public void HookEvents()
    {

    }

    public void Load(object send, EventArgs args)
    {

    }

    public void Save(object send, EventArgs args)
    {

    }

    public virtual void Reset()
    {
        Inventory = null;
        if (HandInventory != null)
        {
            HandInventory.Reset();
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
