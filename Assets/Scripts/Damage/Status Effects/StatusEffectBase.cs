using UnityEngine;

public class StatusEffectBase : ScriptableObject
{
    [field: SerializeField]
    public IEffectable Effectable { get; private set; }

    public string effectName;
    public float duration;
    public float tickSpeed;
    public bool IsDone { get; protected set; }

    public virtual void OnApply(IEffectable effectable) {}
    public virtual void HandleEffect(IEffectable effectable) {}

    public virtual void OnExit(IEffectable effectable) {}
}
