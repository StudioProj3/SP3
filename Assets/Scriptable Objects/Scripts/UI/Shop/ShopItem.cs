using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "Scriptable Objects/Shop Item")]
public class ShopItem : ScriptableObject
{
    [SerializeField, RequireInterface(typeof(ISellable))]
    private Object _sellableItem;

    private ISellable _item; 

    private void OnValidate()
    {
        _item = _sellableItem as ISellable;
    }
}