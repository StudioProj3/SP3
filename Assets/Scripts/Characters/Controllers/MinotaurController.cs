using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurController : 
    CharacterControllerBase, IEffectable
{
    [SerializeField]
    private Stats _minotaurStats;
    [SerializeField]
    private LayerMask _playerLayer;

    private StatContainer _minotaurStatsContainer;

    private GameObject _player;
    private PlayerController _playerController;

    private List<StatusEffectBase> _statusEffects = new();
    private float _currentEffectTime;
    private float _nextTickTime;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    IStatContainer IEffectable.EntityStats => _minotaurStats;

    public void TakeDamage(Damage damage, Vector3 knockback)
    {
        _rigidbody.AddForce(knockback, ForceMode.Impulse);
        _animator.SetBool("isHurt", true);
        damage.OnApply(this);
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



        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds(
            new GenericState("Walk",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position - transform.position;
                    _direction.y = 0;
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _minotaurStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("Spin",
                new ActionEntry("Enter", () =>
                {
                    Collider[] attackTarget;
                    attackTarget = Physics.OverlapCapsule(transform.position,
                        new Vector3(transform.position.x + _direction.x * 0.5f, transform.position.y, transform.position.z),
                        0.35f, _playerLayer, 0);


                    for (int i = 0; i < attackTarget.Length; i++)
                    {

                        if (attackTarget[i].CompareTag("Player"))
                        {

                            Vector3 knockbackForce =
                                (_player.transform.position - transform.position).normalized *
                                _minotaurStatsContainer.GetStat("Knockback").Value;
                            _playerController.TakeDamage(_phyDamage, knockbackForce);
                            break;
                        }
                    }
                })
            ),

            new GenericState("GoingToSpin",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position - transform.position;
                    _direction.y = 0;
                })
            ),

            new GenericState("Quake",
                new ActionEntry("Enter", () =>
                {
                    Collider[] attackTarget;
                    attackTarget = Physics.OverlapCapsule(transform.position,
                        new Vector3(transform.position.x + _direction.x * 0.5f, transform.position.y, transform.position.z),
                        0.35f, _playerLayer, 0);


                    for (int i = 0; i < attackTarget.Length; i++)
                    {

                        if (attackTarget[i].CompareTag("Player"))
                        {

                            Vector3 knockbackForce =
                                (_player.transform.position - transform.position).normalized *
                                _minotaurStatsContainer.GetStat("Knockback").Value;
                            _playerController.TakeDamage(_phyDamage, knockbackForce);
                            break;
                        }
                    }
                })
            ),

            new GenericState("GoingToQuake",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position - transform.position;
                    _direction.y = 0;
                })
            ),


            new GenericState("Cooldown"),

            // Transitions

            // Idle > Walk
            new RandomTimedTransition("Idle", "Walk", 0.2f, 0.5f),

            // Walk > Idle
            new FixedTimedTransition("Walk", "Idle", 0.7f),

            // Idle > GoingToAttack
            new GenericTransition("Idle", "GoingToQuake", () =>
            {
                return _distance < 0.8f;
            }),

            // Walk > GoingToAttack
            new GenericTransition("Walk", "GoingToQuake", () =>
            {
                return _distance < 0.8f;
            }),

            //  GoingToQuake > Quake
            new FixedTimedTransition("GoingToQuake", "Quake", 0.4f),

            // Quake > Cooldown
            new FixedTimedTransition("Quake", "Cooldown", 0.4f),

            // Idle > GoingToSpin
            new GenericTransition("Idle", "GoingToSpin", () =>
            {
                return _distance < 0.8f;
            }),

            // Walk > GoingToSpin
            new GenericTransition("Walk", "GoingToSpin", () =>
            {
                return _distance < 0.8f;
            }),

            //  GoingToSpin > Spin
            new FixedTimedTransition("GoingToSpin", "Spin", 0.4f),

            //  Spin > Idle
            new FixedTimedTransition("Spin", "Idle", 2.0f),

            // Attack > Cooldown
            new FixedTimedTransition("Spin", "Cooldown", 0.4f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 0.2f)

        );

        _stateMachine.SetStartState("Idle");

        _stateMachine.Enter();
    }

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _minotaurStatsContainer = _minotaurStats.GetInstancedStatContainer();
        _phyDamage = PhysicalDamage.Create(_minotaurStatsContainer.
            GetStat("AttackDamage").Value);
    }

    private void Update()
    {


        //_animator.SetBool("isCharging",
        //    _stateMachine.CurrentState.StateID == "GoingToSpin");
        //_animator.SetBool("isSpinning",
        //    _stateMachine.CurrentState.StateID == "Spin");
        //_animator.SetBool("isQuaking",
        //    _stateMachine.CurrentState.StateID == "Quake");
        //_animator.SetBool("isMoving",
        //   _stateMachine.CurrentState.StateID == "Walk");


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
        _distance = Vector3.Distance(_player.transform.position,
            transform.position);

        if (_minotaurStatsContainer.
            GetStat("Health").Value <= 0)
        {
            _animator.SetBool("isDead", true);
        }
        else
            _stateMachine.FixedUpdate();
    }


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == _player)
        {

            Vector3 knockbackForce =
                (col.transform.position - transform.position).normalized *
                _minotaurStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage, knockbackForce);
        }
    }

}
