using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemBaseWrapper",
    menuName = "Scriptable Objects/ItemBase/ItemBaseWrapper")]
public class ItemBaseWrapper : ScriptableObject
{
    [field: SerializeField]
    public ItemBase ItemBase {get; protected set;}

    [field: SerializeField]
    private List<ItemComponentBase> _mutableItemComponents = new();

    public bool TryGetItemComponent<T>(out T itemComponent) where T : ItemComponentBase
    {
        itemComponent = _mutableItemComponents.Find(c => c.GetType() == typeof(T)) as T;

        return itemComponent != null;
    }

    public T GetItemComponent<T>() where T : ItemComponentBase
    {
        return _mutableItemComponents.Find(c => c.GetType() == typeof(T)) as T;
    }
}
