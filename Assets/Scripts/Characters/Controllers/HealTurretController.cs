using UnityEngine;

[DisallowMultipleComponent]
public class HealTurretController :
    EnemyControllerBase, IEffectable
{
    [SerializeField]
    private LayerMask _enemyLayer;

    [SerializeField]
    private float healAmount;

    private StatContainer _healTurretStatsContainer;

    private ParticleSystem _healTurretParticles;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    protected void OnEnable()
    {
        base.Start();

        _healTurretParticles = GetComponentInChildren<ParticleSystem>();
        _healTurretStatsContainer = Data.CharacterStats.
            GetInstancedStatContainer();
        EntityStats = _healTurretStatsContainer;
        _phyDamage = PhysicalDamage.Create(_healTurretStatsContainer.
            GetStat("AttackDamage").Value);

        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds(
            new GenericState("Heal",
                new ActionEntry("Enter", () =>
                {
                    Collider[] healTargets;
                    healTargets = Physics.OverlapSphere(transform.position,
                        4f, _enemyLayer, 0);

                    for (int i = 0; i < healTargets.Length; ++i)
                    {
                        if (healTargets[i].CompareTag("Enemy"))
                        {
                            IStatContainer container = healTargets[i].
                                GetComponent<IEffectable>().EntityStats;

                            container.GetStat("Health").Add(healAmount);
                        }
                    }

                    _healTurretParticles.Play();
                })
            ),

            new GenericState("GoingToHeal"),

            // Transitions

            new AllToOneTransition("Death", () =>
            {
                return _healTurretStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

            // Idle > GoingToHeal
            new FixedTimedTransition("Idle", "GoingToHeal", 0.7f),

            // Idle > Going to charge
            new FixedTimedTransition("GoingToHeal", "Heal", 1f),

            // Heal > Idle
            new FixedTimedTransition("Heal", "Idle", 0.7f)
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
        _animator.SetBool("isHealing",
           _stateMachine.CurrentState.StateID == "GoingToHeal");
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
                _healTurretStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage, knockbackForce);
        }
    }
}
