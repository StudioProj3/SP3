using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData",
    menuName = "Scriptable Objects/Character Data")]
public class CharacterData :
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

    [field: SerializeField]
    public Stats CharacterStats {get; set;}

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
