using System;

using UnityEngine;

// [CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Objects/Shop Item")]
[Serializable]
public class ShopItem : ISerializationCallbackReceiver
{
    [SerializeField, RequireInterface(typeof(ISellable))]
    private ItemBase _sellableItem = null;
    // private UnityEngine.Object _sellableItem = null;

    public ISellable SellableItem { get; private set; } = null;
    public ItemBase Item => _sellableItem;

    public void OnAfterDeserialize()
    {
        SellableItem = _sellableItem as ISellable;
    }

    public void OnBeforeSerialize()
    {
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as ShopItem);
    }

    private bool Equals(ShopItem other)
    {
        if (other == null)
        {
            return false;
        }
        return SellableItem == other.SellableItem;
    }

    public override int GetHashCode()
    {
        return SellableItem?.GetHashCode() ?? 0;
    }
}