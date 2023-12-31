using System.Collections.Generic;

using UnityEngine;

public class MinotaurController :
    EnemyControllerBase, IEffectable
{

    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private StatusEffectBase _earthStatusEffect;

    [SerializeField]
    private AudioClip _sfxSpinning;

    [SerializeField]
    private AudioClip _sfxEarthSpell;

    private GameObject _pooledEarth;
    private List<EarthController> _pooledEarthList;

    private StatContainer _minotaurStatsContainer;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    protected override void OnEnable()
    {
        base.Start();
        base.OnEnable();

        _pooledEarth = transform.GetChild(0).gameObject;
        _pooledEarthList = new List<EarthController>();

        foreach (Transform child in _pooledEarth.transform)
        {
            _pooledEarthList.Add(child.GetComponent<EarthController>());
        }

        _minotaurStatsContainer = Data.CharacterStats.
          GetInstancedStatContainer();
        _phyDamage = PhysicalDamage.Create(_minotaurStatsContainer.
            GetStat("AttackDamage").Value);

        EntityStats = _minotaurStatsContainer;

        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds(
            new GenericState("Walk",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position -
                        transform.position;
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
                    _audioManager.PlaySound3D(_sfxSpinning,
                       transform.position, false);
                }),
        
                new ActionEntry("FixedUpdate", () =>
                {

                    _direction = _player.transform.position -
                       transform.position;

                    _rigidbody.AddForce(_direction.normalized * 0.15f, ForceMode.Impulse);

                    Collider[] attackTarget;
                    attackTarget = Physics.OverlapSphere(transform.position,
                        0.4f, _playerLayer, 0);


                    for (int i = 0; i < attackTarget.Length; i++)
                    {
                        if (!attackTarget[i].CompareTag("Player"))
                        {
                            continue;
                        }

                        Vector3 knockbackForce =
                            (_player.transform.position - transform.position).
                            normalized * _minotaurStatsContainer.
                            GetStat("Knockback").Value;

                        _playerController.TakeDamage(_phyDamage.AddModifier(
                            Modifier.Multiply(_minotaurStatsContainer.
                            GetStat("DamageMultiplier").Value, 3)),
                            knockbackForce);

                        break;
                    }
                })
            ),

            new GenericState("GoingToSpin"),

            new GenericState("Quake",
                new ActionEntry("Enter", () =>
                {

                    _ = Delay.Execute(() =>
                    {
                        EarthSpell(_direction * 0.5f);
                    }, 0.25f);

                    _ = Delay.Execute(() =>
                    {
                        EarthSpell(_direction);
                    }, 0.75f);

                    _ = Delay.Execute(() =>
                    {
                        EarthSpell(_direction * 1.5f);
                    }, 1.25f);

                    _ = Delay.Execute(() =>
                    {
                        EarthSpell(_direction * 2.0f);
                    }, 1.75f);

                    _ = Delay.Execute(() =>
                    {
                        EarthSpell(_direction * 2.5f);
                    }, 2.25f);


                })
            ),

            new GenericState("GoingToQuake",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position -
                        transform.position;
                    _direction.y = 0;
                })
            ),

            new GenericState("Cooldown"),

            // Transitions

            new AllToOneTransition("Death", () =>
            {
                return _minotaurStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

            // Idle > Walk
            new GenericTransition("Idle", "Walk", () =>
            {
                return _distance < 3.0f;
            }),

            // Walk > Idle
            new FixedTimedTransition("Walk", "Idle", 0.7f),

            // Idle > GoingToSpin
            new GenericTransition("Idle", "GoingToSpin", () =>
            {
                return _distance < 1.0f;
            }),

            // Walk > GoingToSpin
            new GenericTransition("Walk", "GoingToSpin", () =>
            {
                return _distance < 1.0f;
            }),

            // Idle > GoingToAttack
            new GenericTransition("Idle", "GoingToQuake", () =>
            {
                return _distance < 1.5f;
            }),

            // Walk > GoingToAttack
            new GenericTransition("Walk", "GoingToQuake", () =>
            {
                return _distance < 1.5f;
            }),

            //  GoingToQuake > Quake
            new FixedTimedTransition("GoingToQuake", "Quake", 0.8f),

            // Quake > Cooldown
            new FixedTimedTransition("Quake", "Cooldown", 0.5f),

            //  GoingToSpin > Spin
            new FixedTimedTransition("GoingToSpin", "Spin", 0.4f),

            //  Spin > Idle
            new FixedTimedTransition("Spin", "Cooldown", 2f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 3.0f)
        );

        _stateMachine.SetStartState("Idle");

        _stateMachine.Enter();
    }

    protected override void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        _animator.SetBool("isCharging",
            _stateMachine.CurrentState.StateID == "GoingToSpin");
        _animator.SetBool("isSpinning",
            _stateMachine.CurrentState.StateID == "Spin");
        _animator.SetBool("isQuaking",
            _stateMachine.CurrentState.StateID == "Quake");
        _animator.SetBool("isMoving",
           _stateMachine.CurrentState.StateID == "Walk");
        _animator.SetBool("isDead",
            _stateMachine.CurrentState.StateID == "Death");

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

        transform.rotation = Quaternion.Euler(0,
            _direction.x < 0 ? 180 : 0, 0);
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_player.transform.position,
            transform.position);

        _stateMachine.FixedUpdate();
        
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == _player)
        {
            Vector3 knockbackForce =
                (col.transform.position - transform.position).normalized *
                _minotaurStatsContainer.GetStat("Knockback").Value;

            _playerController.TakeDamage(_phyDamage.AddModifier(
                Modifier.Multiply(_minotaurStatsContainer.
                GetStat("DamageMultiplier").Value, 3)), knockbackForce);
        }
    }

    private void EarthSpell(Vector3 direction)
    {
        for (int i = 0; i < _pooledEarthList.Count; i++)
        {

            if (!(_pooledEarthList[i].gameObject.activeSelf))
            {
                _audioManager.PlaySound3D(_sfxEarthSpell,
                     transform.position, false);
                _pooledEarthList[i].Init(direction, _phyDamage.AddModifier(
                    Modifier.Multiply(_minotaurStatsContainer.
                    GetStat("DamageMultiplier").Value, 3)),
                    _earthStatusEffect, _pooledEarth.transform);

                _pooledEarthList[i].transform.position =
                    transform.position + direction;
                _pooledEarthList[i].transform.SetParent(null);
                break;

            }
        }
    }
}
