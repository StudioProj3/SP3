using System.Collections.Generic;

using UnityEngine;

// Abstract base class to be inherited from by
// the player and other NPCs
[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterControllerBase :
    MonoBehaviour, IEffectable
{
    [field: SerializeField] 
    public CharacterData Data { get; protected set; }

    public IStatContainer EntityStats { get; protected set; }

    protected Rigidbody _rigidbody;
    protected StateMachine _stateMachine;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected List<StatusEffectBase> _statusEffects = new();

    public virtual void TakeDamage(Damage damage, Vector3 knockback)
    {
        _rigidbody.AddForce(knockback, ForceMode.Impulse);
        damage.OnApply(this);

        Color originalColor = _spriteRenderer.color;

        _spriteRenderer.color = new Color(1, 0, 0,0.5f);
        _ = Delay.Execute(() =>
        {
            _spriteRenderer.color = originalColor;
        }, 0.15f);
    }

    public void ApplyEffect(StatusEffectBase statusEffect)
    {
        _statusEffects.Add(statusEffect);
        statusEffect.OnApply(this);
    }

    public void RemoveEffect(StatusEffectBase statusEffect)
    {
        int index = _statusEffects.IndexOf(statusEffect);
        RemoveEffectImpl(statusEffect, index);
    }
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void SetupStateMachine()
    {
        _stateMachine = new StateMachine("main", 
            new IdleState("Idle")
        );
    }

    protected virtual void Awake()
    {
        Data.HookEvents();
        Data.Reset();
    }
    
    protected void RemoveEffectImpl(StatusEffectBase statusEffect,
        int index)
    {
        statusEffect.OnExit(this);
        _statusEffects.RemoveAt(index);
    }
}
