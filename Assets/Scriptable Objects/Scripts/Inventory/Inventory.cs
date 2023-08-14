using UnityEngine;

[CreateAssetMenu(fileName = "Inventory",
    menuName = "Inventory/Inventory")]
public class Inventory :
    ScriptableObject
{
    public uint NumSlots
    {
        get => _numSlots;
        private set => _numSlots = value;
    }

    public uint MaxPerSlot
    {
        get => _maxPerSlot;
        private set => _maxPerSlot = value;
    }

    [SerializeField]
    [Tooltip("Number of slots in the inventory")]
    private uint _numSlots;

    [SerializeField]
    [Tooltip("Max number of items in 1 slot")]
    private uint _maxPerSlot;
}
