using System.Collections.Generic;

using UnityEngine;

public class NecromancerController :
    CharacterControllerBase, IEffectable
{
    [SerializeField]
    private Stats _necromancerStats;

    private GameObject _pooledSkeletons;
    private List<SkeletonController> _pooledSkeletonList;

    private GameObject _pooledSkulls;
    private List<RedSkullController> _pooledSkullList;

    private StatContainer _necromancerStatsContainer;

    private GameObject _player;
    private PlayerController _playerController;

    private List<StatusEffectBase> _statusEffects = new();
    private float _currentEffectTime;
    private float _nextTickTime;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    IStatContainer IEffectable.EntityStats => _necromancerStats;

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
        _pooledSkeletons = transform.GetChild(0).gameObject;
        _pooledSkeletonList = new List<SkeletonController>();
        _pooledSkulls = transform.GetChild(1).gameObject;
        _pooledSkullList = new List<RedSkullController>();

        foreach (Transform child in _pooledSkulls.transform)
        {
            _pooledSkullList.Add(child.GetComponent<RedSkullController>());
        }

        foreach (Transform child in _pooledSkeletons.transform)
        {
            _pooledSkeletonList.Add(child.GetComponent<SkeletonController>());
        }

        _necromancerStatsContainer = _necromancerStats.GetInstancedStatContainer();
        _phyDamage = PhysicalDamage.Create(_necromancerStatsContainer.
            GetStat("AttackDamage").Value);

        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds(
            new GenericState("Walk",
                new ActionEntry("Enter", () =>
                {
                    _direction = new Vector3(Random.Range(-1.0f, 1.0f),
                        0.0f, Random.Range(-1.0f, 1.0f));
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _necromancerStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("Shoot",
                new ActionEntry("Enter", () =>
                {
                    for (int i = 0; i < _pooledSkullList.Count; i++)
                    {
                        if (!(_pooledSkullList[i].gameObject.activeSelf))
                        {                       
                            _pooledSkullList[i].transform.position =
                                new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f),
                                            transform.position.y - 0.4f,
                                            transform.position.z + Random.Range(-0.5f, 0.5f));

                            _direction = _player.transform.position -
                                        _pooledSkullList[i].transform.position;

                            _pooledSkullList[i].Init(_direction, _phyDamage,
                                _playerController, _pooledSkulls.transform);
                            _pooledSkullList[i].transform.SetParent(null);

                            break;
                        }
                    }
                })
            ),

            new GenericState("Summon",
                new ActionEntry("Enter", () =>
                {
                    for (int i = 0; i < _pooledSkeletonList.Count; i++)
                    {
                        if (!(_pooledSkeletonList[i].gameObject.activeSelf))
                        {
                            _pooledSkeletonList[i].Init( _pooledSkeletons.transform);
                            _pooledSkeletonList[i].transform.position =
                                new Vector3(transform.position.x + Random.Range(-0.75f,0.75f),
                                            transform.position.y - 0.4f,
                                            transform.position.z + Random.Range(-0.75f,0.75f));
                            _pooledSkeletonList[i].transform.SetParent(null);

                            break;
                        }
                    }
                })
            ),

            new GenericState("GoingToSummon"),

            new GenericState("GoingToShoot",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position - transform.position;
                    _direction.y = 0;
                })
            ),

            new GenericState("Cooldown"),

            // Transitions

            // Idle > Walk
            new RandomTimedTransition("Idle", "Walk", 1.0f, 2.0f),

            // Walk > Idle
            new FixedTimedTransition("Walk", "Idle", 0.7f),

            // Idle > Roll
            new GenericTransition("Idle", "GoingToSummon", () =>
            {
                return _distance < 1.0f;
            }),

            // Idle > Going to shoot
            new GenericTransition("Idle", "GoingToShoot", () =>
            {
                return _distance < 3.0f;
            }),

            // Roll > Going to Shoot
            new FixedTimedTransition("GoingToSummon", "Summon", 1.2f),

            //  Going to Shoot > Shoot
            new FixedTimedTransition("GoingToShoot", "Shoot", 1.2f),

            // Shoot > Cooldown
            new FixedTimedTransition("Summon", "Cooldown", 0.2f),

            // Shoot > Cooldown
            new FixedTimedTransition("Shoot", "Cooldown", 0.2f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 1.0f)
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
        _animator.SetBool("isShooting",
            _stateMachine.CurrentState.StateID == "GoingToShoot");
        _animator.SetBool("isMoving",
           _stateMachine.CurrentState.StateID == "Walk");
        _animator.SetBool("isSummoning",
         _stateMachine.CurrentState.StateID == "GoingToSummon");


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

        if (_necromancerStatsContainer.
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
                _necromancerStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage, knockbackForce);
        }
    }


}