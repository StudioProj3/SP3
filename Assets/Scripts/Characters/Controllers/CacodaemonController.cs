using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class CacodaemonController : CharacterControllerBase, IEffectable
{
    [SerializeField]
    private Stats _cacodaemonStats;

    private GameObject _player;

    private List<StatusEffectBase> _statusEffects = new();
    private float _currentEffectTime;
    private float _nextTickTime;

    private Vector3 _direction;

    IStatContainer IEffectable.EntityStats => _cacodaemonStats;

    public void TakeDamage(Damage damage)
    {
        damage.OnApply(_cacodaemonStats);
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

    private void RemoveEffectImpl(StatusEffectBase statusEffect, int index)
    {
        statusEffect.OnExit(this);
        _statusEffects.RemoveAt(index);
    }

    protected override void Start()
    {
        base.Start();

        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds
       (
           new GenericState("Walk",
               new ActionEntry("FixedUpdate", () =>
               {
                   _rigidbody.velocity = _cacodaemonStats.GetStat("MoveSpeed").Value * _direction.normalized;
               })
           ),

           new GenericState("Charge",
               new ActionEntry("Enter", () =>
               {

                   //Vector3 direction =
                       //new(_horizontalInput, 0, _verticalInput);

                   _rigidbody.AddForce(
                       _cacodaemonStats.GetStat("MoveSpeed").Value *
                       2 * _direction.normalized,
                       ForceMode.Impulse
                       );
               })
           ),

           // Transitions
           // Idle > Walk
           new FixedTimedTransition("Idle", "Walk", 0.5f),

           // Walk > Idle
           new FixedTimedTransition("Walk", "Idle", 0.5f),

           // Charge > Idle
           new FixedTimedTransition("Charge", "Idle", 0.7f)

           //// Walk or Idle > Roll
           //new EagerGenericTransition("Walk", "Roll", () =>
           //{
           //    return _rollKeyDown;
           //}),

           //new EagerGenericTransition("Idle", "Roll", () =>
           //{
           //    return _rollKeyDown;
           //})
       );

        _stateMachine.SetStartState("Idle");

        _stateMachine.Enter();
    }

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        _animator.SetBool("isCharging",
            _stateMachine.CurrentState.StateID == "Charge");

        if (!_statusEffects.IsNullOrEmpty())
        {
            _statusEffects.ForEach(effect => effect.HandleEffect(this));
        }

        for (int i = 0; i < _statusEffects.Count; ++i)
        {
            if (_statusEffects[i].IsDone)
            {
                RemoveEffectImpl(_statusEffects[i], i);
                --i;
            }
        }
    }

    private void FixedUpdate()
    {
        Debug.Log(_player.transform.position);
        _direction = _player.transform.position - transform.position;
        _direction.y = 0;
        _stateMachine.FixedUpdate();
    }
}
