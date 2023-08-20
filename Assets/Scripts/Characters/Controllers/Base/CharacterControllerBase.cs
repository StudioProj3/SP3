using System.Collections.Generic;

using UnityEngine;

// Abstract base class to be inherited from by
// the player and other NPCs
[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterControllerBase :
    MonoBehaviour, IEffectable
{
    public IStatContainer EntityStats {get; protected set;}

    protected Rigidbody _rigidbody;
    protected StateMachine _stateMachine;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected List<StatusEffectBase> _statusEffects = new();
    public void TakeDamage(Damage damage, Vector3 knockback)
    {
        _rigidbody.AddForce(knockback, ForceMode.Impulse);
        damage.OnApply(this);

        if (EntityStats.GetStat("Health").Value <= 0)
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }
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
    
    protected void RemoveEffectImpl(StatusEffectBase statusEffect,
        int index)
    {
        statusEffect.OnExit(this);
        _statusEffects.RemoveAt(index);
    }
}
