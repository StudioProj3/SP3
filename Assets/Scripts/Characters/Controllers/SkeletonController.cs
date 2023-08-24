using UnityEngine;

public class SkeletonController :
    EnemyControllerBase, IEffectable
{

    [SerializeField]
    private LayerMask _playerLayer;

    [SerializeField]
    private float _lifetime;

    private float _currentLifetime;

    private StatContainer _skeletonStatsContainer;
    private Transform _source;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    IStatContainer IEffectable.EntityStats =>
        _skeletonStatsContainer;

    public void Init(Transform source)
    {
        gameObject.SetActive(true);
        _currentLifetime = _lifetime;
        _source = source;
    }

    protected void OnEnable()
    {
        base.Start();
        
        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds(
            new GenericState("Init",
                new ActionEntry("Enter", () =>
                {
                    _rigidbody.velocity = new Vector3(0, 0, 0);
                })
            ),

            new GenericState("Walk",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position -
                        transform.position;
                    _direction.y = 0;
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _skeletonStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("Attack",
                new ActionEntry("Enter", () =>
                {
                    Collider[] attackTarget;
                    attackTarget = Physics.OverlapCapsule(transform.position,
                        new Vector3(transform.position.x + _direction.x * 0.5f,
                        transform.position.y, transform.position.z),
                        0.35f, _playerLayer, 0);

                    for (int i = 0; i < attackTarget.Length; i++)
                    {
                        if (!attackTarget[i].CompareTag("Player"))
                        {
                            continue;
                        }

                        Vector3 knockbackForce =
                            (_player.transform.position - transform.position).
                            normalized * _skeletonStatsContainer.
                            GetStat("Knockback").Value;

                        _playerController.TakeDamage(_phyDamage.AddModifier(
                            Modifier.Multiply(_skeletonStatsContainer.
                            GetStat("DamageMultiplier").Value, 3)),
                            knockbackForce);

                        break;
                    }
                })
            ),

            new GenericState("GoingToAttack",
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
                return _skeletonStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

            // Init > Idle
            new FixedTimedTransition("Init", "Idle", 2.0f),

            // Idle > Walk
            new RandomTimedTransition("Idle", "Walk", 0.2f, 0.5f),

            // Walk > Idle
            new FixedTimedTransition("Walk", "Idle", 0.7f),

            // Idle > GoingToAttack
            new GenericTransition("Idle", "GoingToAttack", () =>
            {
                return _distance < 0.8f;
            }),

            // Walk > GoingToAttack
            new GenericTransition("Walk", "GoingToAttack", () =>
            {
                return _distance < 0.8f;
            }),

            //  GoingToAttack > Attack
            new FixedTimedTransition("GoingToAttack", "Attack", 0.4f),

            // Attack > Cooldown
            new FixedTimedTransition("Attack", "Cooldown", 0.4f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 0.2f)
        );

        _stateMachine.SetStartState("Init");

        _stateMachine.Enter();
    }

    protected override void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _skeletonStatsContainer = Data.CharacterStats.
            GetInstancedStatContainer();
        _phyDamage = PhysicalDamage.Create(_skeletonStatsContainer.
            GetStat("AttackDamage").Value);

        EntityStats = _skeletonStatsContainer;
    }

    private void Update()
    {
        _animator.SetBool("isAttacking",
            _stateMachine.CurrentState.StateID == "GoingToAttack");
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

        _currentLifetime -= Time.deltaTime;

        _stateMachine.FixedUpdate();
    }

    private void OnDisable()
    {
        _ = Delay.Execute(() =>
        {
            transform.SetParent(_source);
        }, 0.01f);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == _player)
        {
            Vector3 knockbackForce =
                (col.transform.position - transform.position).normalized *
                _skeletonStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage.AddModifier(
                Modifier.Multiply(_skeletonStatsContainer.
                GetStat("DamageMultiplier").Value, 3)), knockbackForce);
        }
    }
}
