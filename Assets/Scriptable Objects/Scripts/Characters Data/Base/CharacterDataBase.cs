using UnityEngine;

public abstract class CharacterDataBase :
    ScriptableObject, INameable
{
    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public InventoryBase Inventory { get; set; }

    [field: SerializeField]
    public ItemBase LeftHandItem { get; set; }

    [field: SerializeField]
    public ItemBase RightHandItem { get; set; }
}
