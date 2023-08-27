// Modifier to add onto the base or modified value
public class PlusModifier : Modifier
{
    public PlusModifier(float value, int priority = 0, bool permanent = true) :
        base(value, priority, permanent)
    {

    }

    public override float Modify(float value)
    {
        return value + _value;
    }

    protected override Modifier CreateClone(float value,
        int priority, bool permanent = true)
    {
        return new PlusModifier(value, priority, permanent);
    }
}
