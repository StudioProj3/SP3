using System.Collections.Generic;

using UnityEngine;

public class NecromancerController :
    EnemyControllerBase, IEffectable
{

    private GameObject _pooledSkeletons;
    private List<SkeletonController> _pooledSkeletonList;

    private GameObject _pooledSkulls;
    private List<RedSkullController> _pooledSkullList;

    private StatContainer _necromancerStatsContainer;

    private Vector3 _direction;
    private float _distance;
    private MagicDamage _magicDamage;

    protected void OnEnable()
    {
        base.Start();

        _pooledSkeletons = transform.GetChild(0).gameObject;
        _pooledSkeletonList = new List<SkeletonController>();
        _pooledSkulls = transform.GetChild(1).gameObject;
        _pooledSkullList = new List<RedSkullController>();

        foreach (Transform child in _pooledSkulls.transform)
        {
            _pooledSkullList.Add(child.
                GetComponent<RedSkullController>());
        }

        foreach (Transform child in _pooledSkeletons.transform)
        {
            _pooledSkeletonList.Add(child.
                GetComponent<SkeletonController>());
        }

        _necromancerStatsContainer = Data.CharacterStats.
            GetInstancedStatContainer();
        EntityStats = _necromancerStatsContainer;
        _magicDamage = MagicDamage.Create(_necromancerStatsContainer.
            GetStat("AbilityPower").Value);

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
                            float xRand = Random.Range(-0.5f, 0.5f);
                            float zRand = Random.Range(-0.5f, 0.5f);

                            _pooledSkullList[i].transform.position =
                                new Vector3(transform.position.x + xRand,
                                transform.position.y - 0.4f,
                                transform.position.z + zRand);

                            _direction = _player.transform.position -
                                _pooledSkullList[i].transform.position;

                            _pooledSkullList[i].Init(_direction, _magicDamage,
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
                            float xRand = Random.Range(-0.75f, 0.75f);
                            float zRand = Random.Range(-0.75f, 0.75f);

                            _pooledSkeletonList[i].Init(_pooledSkeletons.
                                transform);
                            _pooledSkeletonList[i].transform.position =
                                new Vector3(transform.position.x + xRand,
                                transform.position.y - 0.4f,
                                transform.position.z + zRand);
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
                    _direction = _player.transform.position -
                        transform.position;
                    _direction.y = 0;
                })
            ),

            new GenericState("Cooldown"),

            // Transitions

            new AllToOneTransition("Death", () =>
            {
                return _necromancerStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

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

    protected override void Awake()
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
                _necromancerStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_magicDamage, knockbackForce);
        }
    }
}
