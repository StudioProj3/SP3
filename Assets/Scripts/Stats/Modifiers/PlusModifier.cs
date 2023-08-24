// Modifier to add onto the base or modified value
public class PlusModifier : Modifier
{
    public PlusModifier(float value, int priority = 0) :
        base(value, priority)
    {

    }

    public override float Modify(float value)
    {
        return value + this.value;
    }

    protected override Modifier CreateClone(float value,
        int priority)
    {
        return new PlusModifier(value, priority);
    }
}
