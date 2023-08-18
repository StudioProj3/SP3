using UnityEngine;

public abstract class Damage : ScriptableObject
{
    [field: SerializeField]
    protected float _damage;

    public abstract void OnApply(IEffectable entityStats);
    public virtual void AfterApply() {}
}
