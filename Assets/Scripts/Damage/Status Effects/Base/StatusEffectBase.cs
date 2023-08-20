using UnityEngine;

public abstract class StatusEffectBase : ScriptableObject
{
    [field: SerializeField]
    public IEffectable Effectable { get; private set; }

    public bool IsDone { get; protected set; }

    [SerializeField]
    protected string _effectName;

    [SerializeField]
    protected float _duration;

    [SerializeField]
    protected float _tickSpeed;

    public virtual void OnApply(IEffectable effectable)
    {

    }

    public virtual void HandleEffect(IEffectable effectable)
    {

    }

    public virtual void OnExit(IEffectable effectable)
    {

    }

    public abstract StatusEffectBase Clone();
}
