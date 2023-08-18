using System.Collections.Generic;

using UnityEngine;

[DisallowMultipleComponent]
public class HealTurretController :
    CharacterControllerBase, IEffectable
{
    [SerializeField]
    private Stats _healTurretStats;

    [SerializeField]
    private LayerMask enemyLayer;

    [SerializeField]
    private float healAmount;

    private StatContainer _healTurretStatsContainer;

    private ParticleSystem _healTurretParticles;

    private GameObject _player;
    private PlayerController _playerController;

    private List<StatusEffectBase> _statusEffects = new();
    private float _currentEffectTime;
    private float _nextTickTime;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    IStatContainer IEffectable.EntityStats => _healTurretStats;

    public void TakeDamage(Damage damage, Vector3 knockback)
    {
        _rigidbody.AddForce(knockback, ForceMode.Impulse);
        damage.OnApply(this);
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

        _healTurretParticles = GetComponentInChildren<ParticleSystem>();
        _healTurretStatsContainer = _healTurretStats.
            GetInstancedStatContainer();
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
                        4f, enemyLayer, 0);

                    for (int i = 0; i < healTargets.Length; ++i)
                    {
                        if (healTargets[i].CompareTag("Enemy"))
                        {
                            IStatContainer container = healTargets[i].
                                GetComponent<IEffectable>().EntityStats;
                            container.GetStat("Health").Add(healAmount);
                        }
                    }

                    float angle = -Mathf.Atan2(_direction.z, _direction.x) *
                        Mathf.Rad2Deg;

                    _healTurretParticles.Play();
                })
            ),

            new GenericState("GoingToHeal"),

            // Transitions

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

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        _playerController = _player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        _animator.SetBool("isHealing",
           _stateMachine.CurrentState.StateID == "GoingToHeal");

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

        _spriteRenderer.flipX = _direction.x < 0;
    }

    private void FixedUpdate()
    {
        _distance = Vector3.Distance(_player.transform.position, transform.position);
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
