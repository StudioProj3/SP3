using UnityEngine;

[CreateAssetMenu(fileName = "Wood",
    menuName = "Scriptable Objects/Items/Wood")]
public class WoodItem : ItemBase
{
    [field: SerializeField]
    public bool Rotten { get; protected set; }
}
