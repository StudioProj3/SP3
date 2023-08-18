using UnityEngine;

public abstract class CharacterDataBase :
    ScriptableObject, INameable
{
    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public InventoryBase Inventory { get; set; }

    [field: SerializeField]
    public HandInventory HandInventory { get; set; }

    [field: SerializeField]
    public bool IsDead { get; set; }

    public virtual void Reset()
    {
        Inventory = null;
        HandInventory.Reset();
        IsDead = false;
    }
}
