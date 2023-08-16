using UnityEngine;

[CreateAssetMenu(fileName = "Wood",
    menuName = "Items/Wood")]
public class WoodItem : ItemBase, ISellable
{
    [field: SerializeField]
    public bool Rotten { get; protected set; }

    [field: SerializeField]
    public CurrencyCost CurrencyCost { get; protected set; }

}
