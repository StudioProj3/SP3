using UnityEngine;

[CreateAssetMenu(fileName = "Wood",
    menuName = "Scriptable Objects/Items/Wood")]
public class WoodItem :
    ItemBase, IItemDecay
{
    [field: HorizontalDivider]
    [field: Header("Item Parameters")]

    [field: SerializeField]
    public bool Rotten { get; protected set; }

    [field: SerializeField]
    public CurrencyCost CurrencyCost { get; protected set; }

    [field: HorizontalDivider]
    [field: Header("Decay Parameters")]

    [field: SerializeField]
    public IItemDecay.DecayDuration DurationType
        { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("DurationType", IItemDecay.DecayDuration.Fixed,
        true)]
    public float FixedDuration { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("DurationType", IItemDecay.DecayDuration.Random,
        true)]
    public float MinDuration { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("DurationType", IItemDecay.DecayDuration.Random,
        true)]
    public float MaxDuration { get; protected set; }

    [field: SerializeField]
    public ItemBase Target { get; protected set; }

    public WoodItem()
    {
        Name = "Wood";
    }
}
