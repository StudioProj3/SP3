using UnityEngine;

[CreateAssetMenu(fileName = "Wood",
    menuName = "Items/Wood")]
public class WoodItem : ItemBase
{
    [field: SerializeField]
    public bool Rotten { get; protected set; }
}
