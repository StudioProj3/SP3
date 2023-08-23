using UnityEngine;

public class WitchController :
    EnemyControllerBase, IEffectable
{

    [SerializeField]
    private LayerMask _enemyLayer;

    [SerializeField]
    private StatusEffectBase _attackBuff;

    [SerializeField]
    private StatusEffectBase _defenseBuff;

    private StatContainer _witchStatsContainer;

    private ParticleSystem _witchParticles;

    private GameObject _target;
    private float _targetHealth;

    private Vector3 _direction;
    private float _distance;
    private MagicDamage _magicDamage;
    private float buffCooldown;

    protected override void Start()
    {
        base.Start();

        _witchParticles = GetComponentInChildren<ParticleSystem>();
        _witchStatsContainer = Data.CharacterStats.
            GetInstancedStatContainer();
        EntityStats = _witchStatsContainer;
        _magicDamage = MagicDamage.Create(_witchStatsContainer.
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

                    float highestHealth = 0;
                    Collider[] buffTargets;
                    buffTargets = Physics.OverlapSphere(transform.position,
                                    3f, _enemyLayer, 0);

                    for (int i = 0; i < buffTargets.Length; ++i)
                    {
                        if (buffTargets[i].CompareTag("Enemy"))
                        {
                            IStatContainer container = buffTargets[i].
                                GetComponent<IEffectable>().EntityStats;


                            if (container.GetStat("Health").Value
                                > highestHealth)
                            {
                                _target = buffTargets[i].gameObject;
                                _targetHealth = container.GetStat("Health")
                                    .Value;
                            }
                        }
                    }

                    if (_target)
                    {
                        _direction = _target.transform.position -
                            transform.position;
                        _direction.y = 0f;
                    }
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _witchStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("Charging"),

            new GenericState("Buff"),

            new GenericState("BuffAttack",
                new ActionEntry("Enter", () =>
                {
                    // FIXME (Aquila) When getting damage multiplier
                    // stat after applying buff, an error occurs

                    Collider[] buffTargets;
                    buffTargets = Physics.OverlapSphere(transform.position,
                                    2f, _enemyLayer, 0);

                    for (int i = 0; i < buffTargets.Length; ++i)
                    {
                        if (buffTargets[i].CompareTag("Enemy"))
                        {
                            IEffectable container = buffTargets[i].
                                GetComponent<IEffectable>();

                            container.ApplyEffect(_attackBuff.Clone());
                        }
                    }
                    var particleMain = _witchParticles.main;
                    particleMain.startColor = new Color(
                        1.0f, 0.4f, 0);
                    _witchParticles.Play();

                    buffCooldown = 10.0f;
                })
            ),

             new GenericState("BuffDefense",
                new ActionEntry("Enter", () =>
                {

                    // FIXME (Aquila) When getting armor stat
                    // after applying buff, an error occurs

                    Collider[] buffTargets;
                    buffTargets = Physics.OverlapSphere(transform.position,
                                    2f, _enemyLayer, 0);

                    for (int i = 0; i < buffTargets.Length; ++i)
                    {
                        if (buffTargets[i].CompareTag("Enemy"))
                        {
                            IEffectable container = buffTargets[i].
                                GetComponent<IEffectable>();

                            container.ApplyEffect(_defenseBuff.Clone());
                        }
                    }

                    var particleMain = _witchParticles.main;
                    particleMain.startColor = new Color(
                        0, 0.4f, 1.0f);
                    _witchParticles.Play();

                    buffCooldown = 10.0f;

                })
            ),


            new GenericState("Cooldown"),

            // Transitions

            new AllToOneTransition("Death", () =>
            {
                return _witchStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

            // Idle > Walk
            new RandomTimedTransition("Idle", "Walk", 0.5f, 1f),

            // Walk > Idle
            new FixedTimedTransition("Walk", "Idle", 1.0f),

            // Idle > Going to charging
            new GenericTransition("Idle", "Charging", () =>
            {
                return _distance < 2.0f && buffCooldown < 0.0f;
            }),

            // Charge > Cooldown
            new FixedTimedTransition("Charging", "Buff", 1.0f),

            // Buff > Buff Defense
            new GenericTransition("Buff", "BuffDefense", () =>
            {
                return _targetHealth <= 30;
            }),

            // Buff > Buff Defense
            new GenericTransition("Buff", "BuffAttack", () =>
            {
                return _targetHealth > 30;
            }),

            // BuffDefense > Cooldown
            new FixedTimedTransition("BuffDefense", "Cooldown", 0.7f),

            // BuffAttack > Cooldown
            new FixedTimedTransition("BuffAttack", "Cooldown", 0.7f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 0.7f)
        );

        _stateMachine.SetStartState("Walk");

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
        _animator.SetBool("isCharging",
            _stateMachine.CurrentState.StateID == "Charging");
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
                i--;
            }
        }

        transform.rotation = Quaternion.Euler(0,
            _direction.x < 0 ? 180 : 0, 0);

    }

    private void FixedUpdate()
    {
        buffCooldown -= Time.deltaTime;
        if (_target)
        {
            _distance = Vector3.Distance(_target.transform.position,
                transform.position);
        }
        else
        {
            _distance = 99.0f;
        }

        _stateMachine.FixedUpdate();
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == _player)
        {
            Vector3 knockbackForce =
                (col.transform.position - transform.position).normalized *
                _witchStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_magicDamage.AddModifier(
                Modifier.Multiply(_witchStatsContainer.
                GetStat("DamageMultiplier").Value, 3)),
                knockbackForce);
        }
    }
}
