using System.Collections.Generic;

using UnityEngine;

public class BossController :
    EnemyControllerBase, IEffectable
{
    [SerializeField]
    private LayerMask _weaponLayer;

    [SerializeField]
    private StatusEffectBase _arrowStatusEffect;

    private GameObject _pooledMissles;
    private List<MissleController> _pooledMissleList;

    private GameObject _pooledBullets;
    private List<ArrowController> _pooledBulletList;

    private LaserController _pooledLasers;


    private StatContainer _bossStatsContainer;

    private GameObject _player;
    private PlayerController _playerController;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;
    private MagicDamage _magicDamage;

    protected override void Start()
    {
        base.Start();
        _pooledMissles = transform.GetChild(0).gameObject;
        _pooledMissleList = new List<MissleController>();

        _pooledBullets = transform.GetChild(1).gameObject;
        _pooledBulletList = new List<ArrowController>();

        _pooledLasers = transform.GetChild(2).GetComponent<LaserController>();

        foreach (Transform child in _pooledMissles.transform)
        {
            _pooledMissleList.Add(child.GetComponent<MissleController>());
        }

        foreach (Transform child in _pooledBullets.transform)
        {
            _pooledBulletList.Add(child.GetComponent<ArrowController>());
        }

        _bossStatsContainer = Data.CharacterStats.
            GetInstancedStatContainer();

        EntityStats = _bossStatsContainer;
        _phyDamage = PhysicalDamage.Create(_bossStatsContainer.
            GetStat("AttackDamage").Value);
        _magicDamage = MagicDamage.Create(_bossStatsContainer.
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
                    _rigidbody.velocity = _bossStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("ShootMissle",
                new ActionEntry("Enter", () =>
                {
                    for (int i = 0; i < _pooledMissleList.Count; i++)
                    {
                        if (!(_pooledMissleList[i].gameObject.activeSelf))
                        {
                            _pooledMissleList[i].Init(_direction, _phyDamage,
                                _arrowStatusEffect, _pooledMissles.transform, _player);
                            _pooledMissleList[i].transform.position =
                                transform.position;
                            _pooledMissleList[i].transform.SetParent(null);

                            break;
                        }
                    }
                })
            ),

            new GenericState("ShootRapid",
                new ActionEntry("Enter", () =>
                {

                    for (int i = 0; i < 5; i++)
                    {
                        _ = Delay.Execute(() =>
                        {
                            ShootBullet();
                        }, i * 0.2f);
                    }
                })
            ),

            new GenericState("ShootLaser",
                new ActionEntry("Enter", () =>
                {
                    _pooledLasers.Init(_direction, _magicDamage,
                               _playerController);
                }),

                new ActionEntry("FixedUpdate", () =>
                {
                    _pooledLasers.transform.Rotate(0.0f, 2.0f, 0.0f);
                })
            ),

            new GenericState("Shield",
                new ActionEntry("FixedUpdate", () =>
                {
                    Collider[] weaponTarget;
                    weaponTarget = Physics.OverlapSphere(transform.position,
                        0.5f, _weaponLayer, 0);


                    for (int i = 0; i < weaponTarget.Length; i++)
                    {
                        Vector3 knockbackForce =
                            (_player.transform.position - transform.position).
                            normalized * _bossStatsContainer.
                            GetStat("Knockback").Value;

                        if (!weaponTarget[i].CompareTag("MeleeWeapon"))
                        {
                            _playerController.TakeDamage(
                                _phyDamage, knockbackForce);
                        }
                        else if (!weaponTarget[i].CompareTag("RangedWeapon"))
                        {
                            weaponTarget[i].gameObject.SetActive(false);
                            ShootBullet();
                        }
                    }
                })
            ),

            new GenericState("MeleeDash",
                new ActionEntry("Enter", () =>
                {

                    for (int i = 0; i < 3; i++)
                    {
                        _ = Delay.Execute(() =>
                        {
                            _direction = _player.transform.position -
                                transform.position;
                            _direction.y = 0;

                            _rigidbody.AddForce(_bossStatsContainer.
                                GetStat("MoveSpeed").Value * 6f * _direction.normalized,
                                ForceMode.Impulse);
                        }, i);
                    }
                    
                })
            ),

            new GenericState("MeleeDashStart"),

            new GenericState("MeleeDashEnd"),

            new GenericState("Cooldown"),

            // Transitions

            new AllToOneTransition("Death", () =>
            {
                return _bossStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

            // Idle > Walk
            //new RandomTimedTransition("Idle", "Walk", 1.0f, 2.0f),

            new FixedTimedTransition("Idle", "ShootMissle", 2.0f),

             new FixedTimedTransition("ShootMissle", "Idle", 5.0f),

            // Walk > Idle
            new FixedTimedTransition("Walk", "Idle", 0.7f),

            // Idle > Roll
            new GenericTransition("Idle", "MeleeDashStart", () =>
            {
                return _distance < 1.0f;
            }),

            // Walk > Idle
            new FixedTimedTransition("MeleeDashStart", "MeleeDash", 0.5f),

            // Walk > Idle
            new FixedTimedTransition("MeleeDash", "MeleeDashEnd", 3.0f),

            // Walk > Idle
            new FixedTimedTransition("MeleeDashEnd", "Idle", 0.5f),

            // Idle > Going to shoot
            new GenericTransition("Walk", "ShootMissle", () =>
            {
                return _distance < 3.0f;
            }),

            // Roll > Going to Shoot
            new FixedTimedTransition("Shield", "ShootMissle", 0.5f),


            // Shoot > Cooldown
            new FixedTimedTransition("ShootMissle", "Cooldown", 0.2f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 0.2f)
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
        _animator.SetBool("isMoving",
            _stateMachine.CurrentState.StateID == "Walk");
        _animator.SetBool("isMissle",
            _stateMachine.CurrentState.StateID == "ShootMissle");
        _animator.SetBool("isRapid",
            _stateMachine.CurrentState.StateID == "ShootRapid");
        _animator.SetBool("isLaser",
            _stateMachine.CurrentState.StateID == "ShootLaser");
        _animator.SetBool("isMeleeStarting",
            _stateMachine.CurrentState.StateID == "MeleeDashStart");
        _animator.SetBool("isMeleeDashing",
            _stateMachine.CurrentState.StateID == "MeleeDash");
        _animator.SetBool("isMeleeEnding",
            _stateMachine.CurrentState.StateID == "MeleeDashEnd");
        _animator.SetBool("isShield",
            _stateMachine.CurrentState.StateID == "Shield");
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
                _bossStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage, knockbackForce);
        }
    }

    private void ShootBullet()
    {
        _direction = _player.transform.position -
                      transform.position;
        _direction.y = 0;
        for (int j = 0; j < _pooledBulletList.Count; j++)
        {
            if (!(_pooledBulletList[j]
                .gameObject.activeSelf))
            {
                _pooledBulletList[j].Init(_direction, _phyDamage,
                    _arrowStatusEffect, _pooledMissles.transform);
                _pooledBulletList[j].transform.position = transform.position;
                _pooledBulletList[j].transform.SetParent(null);

                break;
            }
        }
    }
}
