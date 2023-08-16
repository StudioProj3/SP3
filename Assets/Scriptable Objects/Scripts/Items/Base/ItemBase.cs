using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBase",
    menuName = "Scriptable Objects/ItemBase/ItemBase")]
public class ItemBase :
    ScriptableObject, INameable
{
    [field: SerializeField]
    public string Name { get; protected set; }

    [field: SerializeField]
    public Sprite Sprite { get; protected set; }

    [field: SerializeField]
    public bool Stackable { get; protected set; }

    [field: SerializeField]
    public float Weight { get; protected set; } = 0f;

    [SerializeReference]
    private List<ItemComponentBase> _itemComponents = new();

    // TODO (BRAND): Make it immutable.
    [TextArea]
    public string Description;

    public bool TryGetItemComponent<T>(out T itemComponent) where T : ItemComponentBase
    {
        itemComponent = _itemComponents.Find(c => c.GetType() == typeof(T)) as T;

        return itemComponent != null;
    }

    public T GetItemComponent<T>() where T : ItemComponentBase
    {
        return _itemComponents.Find(c => c.GetType() == typeof(T)) as T;
    }
}
