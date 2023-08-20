using UnityEngine;

public abstract class Damage : ScriptableObject
{
    [SerializeField]
    protected float _damage;

    public abstract void OnApply(IEffectable entityStats);

    public virtual void AfterApply()
    {

    }
}
