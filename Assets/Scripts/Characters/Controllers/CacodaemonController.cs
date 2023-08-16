using System.Collections.Generic;
using UnityEngine;
[DisallowMultipleComponent]
public class CacodaemonController : CharacterControllerBase, IEffectable
{
   
   
    [SerializeField]
    private Stats _cacodaemonStats;

    private StatContainer _cacodaemonStatsContainer;

    private ParticleSystem _cacodaemonParticles;

    private GameObject _player;
    private PlayerController _playerController;

    private List<StatusEffectBase> _statusEffects = new();
    private float _currentEffectTime;
    private float _nextTickTime;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    IStatContainer IEffectable.EntityStats => _cacodaemonStats;

    public void TakeDamage(Damage damage)
    {
        _animator.SetBool("isHurt", true);
        damage.OnApply(_cacodaemonStats);
        _animator.SetBool("isHurt", false);

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
        _cacodaemonParticles = GetComponentInChildren<ParticleSystem>();
        _cacodaemonStatsContainer = _cacodaemonStats.GetInstancedStatContainer();
        _phyDamage = PhysicalDamage.Create(_cacodaemonStatsContainer.GetStat("AttackDamage").Value);
        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds
       (
           new GenericState("Walk",
               new ActionEntry("Enter", () =>
               {
                   _direction = new Vector3(Random.Range(-1.0f, 1.0f),
                                            0.0f,
                                            Random.Range(-1.0f, 1.0f));

               }),
               new ActionEntry("FixedUpdate", () =>
               {

                   _rigidbody.velocity = _cacodaemonStatsContainer.GetStat("MoveSpeed").Value * _direction;
               })
           ),

           new GenericState("Charge",
               new ActionEntry("Enter", () =>
               {

                   _direction = _player.transform.position - transform.position;
                   _direction.y = 0;

                   // FIXME (Aquila): Bug where rotation of particles is
                   // not accurate, likely due to Vector3.Angle() giving
                   // the smallest angle possible between the source and
                   // target.

                   float angle = -Mathf.Atan2(_direction.z, _direction.x) * Mathf.Rad2Deg;

                   _cacodaemonParticles.transform.rotation = Quaternion.Euler(0, angle, 0);
                   _cacodaemonParticles.Play();

                   _rigidbody.AddForce(
                       _cacodaemonStatsContainer.GetStat("MoveSpeed").Value *
                       3 * _direction.normalized,
                       ForceMode.Impulse
                       );
               })
           ),

           new GenericState("GoingToCharge"),

            new GenericState("Cooldown"),

           // Transitions

           // Idle > Walk
           new RandomTimedTransition("Idle", "Walk", 1.0f,2.0f),

           // Walk > Idle
           new FixedTimedTransition("Walk", "Idle", 0.7f),

           // Idle > Going to charge
           new GenericTransition("Idle", "GoingToCharge", () =>
           {
               return _distance < 1.0f;
           }),

          // Idle > Going to charge
          new FixedTimedTransition("GoingToCharge", "Charge", 0.5f),

           // Charge > Cooldown
           new FixedTimedTransition("Charge", "Cooldown", 0.7f),

           // Cooldown > Idle
           new FixedTimedTransition("Cooldown", "Idle", 0.7f)

       );

        _stateMachine.SetStartState("Idle");

        _stateMachine.Enter();
    }

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        _animator.SetBool("isCharging",
            _stateMachine.CurrentState.StateID == "Charge");
        _animator.SetBool("isGoingCharge",
           _stateMachine.CurrentState.StateID == "GoingToCharge");

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
            
        _spriteRenderer.flipX = _direction.x < 0;


    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_player.transform.position, transform.position);
        _stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.gameObject == _player)
        {
            _playerController.TakeDamage(_phyDamage);
        }
        
    }

}
