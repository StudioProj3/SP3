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
    public bool Disable { get; protected set; } = false;

    [field: SerializeField]
    [field: ShowIf("Disable", false, true)]
    public IItemDecay.DecayDuration DurationType
        { get; protected set; } = IItemDecay.DecayDuration.Fixed;

    [field: SerializeField]
    [field: ShowIf("_fixedDurationCond", true)]
    public float FixedDuration { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("_randomDurationCond", true)]
    public float MinDuration { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("_randomDurationCond", true)]
    public float MaxDuration { get; protected set; }

    [field: SerializeField]
    [field: ShowIf("Disable", false, true)]
    public ItemBase Target { get; protected set; }

    // For use as implementation detail
    [SerializeField]
    [HideInInspector]
    protected bool _fixedDurationCond = true;

    // For use as implementation detail
    [SerializeField]
    [HideInInspector]
    protected bool _randomDurationCond = false;

    public WoodItem()
    {
        Name = "Wood";
    }

    private void OnValidate()
    {
        _fixedDurationCond = !Disable &&
            DurationType == IItemDecay.DecayDuration.Fixed;
        _randomDurationCond = !Disable &&
            DurationType == IItemDecay.DecayDuration.Random;
    }
}
