using UnityEngine;

public abstract class Damage : ScriptableObject
{
    [SerializeField]
    protected float _damage;

    public abstract void OnApply(IStatContainer entityStats);
    public virtual void AfterApply() {}
}
