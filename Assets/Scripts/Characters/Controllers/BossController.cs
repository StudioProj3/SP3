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

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;
    private MagicDamage _magicDamage;

    private int _attackIndex;

    protected void OnEnable()
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
                    int randomNum = Random.Range(0, 2);
                    if (randomNum == 0)
                    {
                        _direction = new Vector3(Random.Range(-1.0f, 1.0f),
                            0.0f, Random.Range(-1.0f, 1.0f));
                    }
                    else if(randomNum == 1)
                    {
                        _direction = _player.transform.position -
                            transform.position;
                    }
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _bossStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("Attack"),

            new GenericState("ShootMissle",
                new ActionEntry("Enter", () =>
                {
                    _direction = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));

                    for (int i = 0; i < _pooledMissleList.Count; i++)
                    {
                        if (!(_pooledMissleList[i].gameObject.activeSelf))
                        {
                            _pooledMissleList[i].Init(_direction, _phyDamage.AddModifier(
                                Modifier.Multiply(_bossStatsContainer.
                                GetStat("DamageMultiplier").Value, 3)),
                                _arrowStatusEffect, _pooledMissles.transform, _player);

                            _pooledMissleList[i].transform.position =
                                transform.position;
                            _pooledMissleList[i].transform.SetParent(null);

                            break;
                        }
                    }

                    for (int i = 0; i < _pooledMissleList.Count; i++)
                    {
                        if (!(_pooledMissleList[i].gameObject.activeSelf))
                        {
                            _pooledMissleList[i].Init(-_direction, _phyDamage.AddModifier(
                                Modifier.Multiply(_bossStatsContainer.
                                GetStat("DamageMultiplier").Value, 3)),
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

            new GenericState("ShootLaserStart",
                new ActionEntry("Enter", () =>
                {
                    _pooledLasers.transform.position = transform.position;
                    _pooledLasers.Init(_direction, _magicDamage.AddModifier(
                        Modifier.Multiply(0.5f,3)),
                        _playerController);
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _pooledLasers.transform.position = transform.position;

                })
            ),

            new GenericState("ShootLaser",
                new ActionEntry("Enter", () =>
                {
                    _pooledLasers.SetCollide(true);
                    _pooledLasers.SetAnimatorBool("Firing", true);
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _pooledLasers.transform.Rotate(0.0f, 1.5f, 0.0f);
                    _pooledLasers.transform.position = transform.position;

                })
            ),

            new GenericState("ShootLaserEnd",
                new ActionEntry("Enter", () =>
                {
                    _pooledLasers.SetCollide(false);

                    _pooledLasers.SetAnimatorBool("End", true);
                })
            ),

            new GenericState("ShootLaserCooldown",
                new ActionEntry("Enter", () =>
                {
                    _pooledLasers.SetAnimatorBool("Firing", false);
                    _pooledLasers.SetAnimatorBool("End", false);
                })
            ),

            new GenericState("Shield",
                new ActionEntry("FixedUpdate", () =>
                {
                    //Collider[] weaponTarget;
                    //weaponTarget = Physics.OverlapSphere(transform.position,
                    //    1.0f, _weaponLayer, QueryTriggerInteraction.Collide);


                    //for (int i = 0; i < weaponTarget.Length; i++)
                    //{
                    //    Vector3 knockbackForce =
                    //        (_player.transform.position - transform.position).
                    //        normalized * _bossStatsContainer.
                    //        GetStat("Knockback").Value;

                    //    _playerController.TakeDamage(
                    //            _phyDamage, knockbackForce);

                    //    Debug.Log("bruh");

                    //}
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
            new RandomTimedTransition("Idle", "Walk", 0.5f, 1.0f),

            new RandomTimedTransition("Walk", "Attack", 1.0f, 2.0f),

            // Idle > Roll
            new GenericTransition("Attack", "MeleeDashStart", () =>
            {
                return _distance < 1.0f || _attackIndex == 4;
            }),

            // Walk > Idle
            new FixedTimedTransition("MeleeDashStart", "MeleeDash", 0.5f),

            // Walk > Idle
            new FixedTimedTransition("MeleeDash", "MeleeDashEnd", 3.0f),

            // Walk > Idle
            new FixedTimedTransition("MeleeDashEnd", "Cooldown", 0.5f),

            new GenericTransition("Attack", "ShootLaserStart", () =>
            {
                return _attackIndex == 0;
            }),

            new FixedTimedTransition("ShootLaserStart", "ShootLaser", 0.6f),

            new FixedTimedTransition("ShootLaser", "ShootLaserEnd", 4.0f),

            new FixedTimedTransition("ShootLaserEnd", "Cooldown", 2.0f),

            new GenericTransition("Attack", "ShootRapid", () =>
            {
                 return _attackIndex == 1;
            }),

            new FixedTimedTransition("ShootRapid", "Cooldown", 2.0f),

            new GenericTransition("Attack", "ShootMissle", () =>
            {
                return _attackIndex == 2;
            }),

            // Roll > Going to Shoot
            //new FixedTimedTransition("Shield", "ShootMissle", 0.5f),

            //new FixedTimedTransition("Shield", "Idle", 0.5f),

            // Shoot > Cooldown
            new FixedTimedTransition("ShootMissle", "Cooldown", 0.2f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 0.5f)
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

        _attackIndex = Random.Range(0, 4);
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
            _playerController.TakeDamage(_phyDamage.AddModifier(
                Modifier.Multiply(_bossStatsContainer.
                GetStat("DamageMultiplier").Value, 3)), knockbackForce);
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
                _pooledBulletList[j].Init(_direction, _magicDamage.AddModifier(
                    Modifier.Multiply(_bossStatsContainer.
                    GetStat("DamageMultiplier").Value, 3)),
                    _arrowStatusEffect, _pooledBullets.transform);

                _pooledBulletList[j].transform.position = new Vector3(
                    transform.position.x,
                    _pooledBullets.transform.position.y,
                    transform.position.z);

                _pooledBulletList[j].transform.SetParent(null);

                break;
            }
        }
    }
}
