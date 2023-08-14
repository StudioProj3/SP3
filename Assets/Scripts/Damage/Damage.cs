using UnityEngine;
public abstract class Damage : ScriptableObject
{
    [SerializeField]
    protected float _amount;

    public abstract void OnApply();
    public virtual void AfterApply() {}
}
