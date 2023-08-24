using UnityEngine;

public abstract class Damage : ScriptableObject
{
    [SerializeField]
    protected float _initialDamageValue;

    protected ModifiableValue _damage;

    public abstract void OnApply(IEffectable entityStats);

    public virtual void AfterApply()
    {
    }

    public virtual Damage AddModifier(Modifier modifier)
    {
        if (modifier != null)
        {
           _damage.AddModifier(modifier);
        }
        return this;
    } 

    public virtual Damage RemoveModifier(Modifier modifier)
    {
        if (modifier != null)
        {
           _damage.RemoveModifier(modifier);
        }
        return this;
    }

    protected virtual void OnValidate()
    {
        _damage = new ModifiableValue(_initialDamageValue);
    }
}
