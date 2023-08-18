using UnityEngine;

// Abstract base class to be inherited from by
// the player and other NPCs
[RequireComponent(typeof(Rigidbody))]
public abstract class CharacterControllerBase :
    MonoBehaviour
{
    protected Rigidbody _rigidbody;
    protected StateMachine _stateMachine;
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

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
}
