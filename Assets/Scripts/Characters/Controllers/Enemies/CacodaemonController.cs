using UnityEngine;

[DisallowMultipleComponent]
public class CacodaemonController :
    EnemyControllerBase, IEffectable
{
    [SerializeField]
    private AudioClip _sfxGoingToCharge;
    [SerializeField]
    private AudioClip _sfxCharging;

    private StatContainer _cacodaemonStatsContainer;

    private ParticleSystem _cacodaemonParticles;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;
    
    protected void OnEnable()
    {
        base.Start();

        _cacodaemonParticles = GetComponentInChildren<ParticleSystem>();
        _cacodaemonStatsContainer = Data.CharacterStats.
            GetInstancedStatContainer();
        EntityStats = _cacodaemonStatsContainer;
        _phyDamage = PhysicalDamage.Create(_cacodaemonStatsContainer.
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
                    _direction = new Vector3(UnityEngine.Random.Range(-1f, 1f),
                        0f, UnityEngine.Random.Range(-1f, 1f));
                }),
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _cacodaemonStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("Charge",
                new ActionEntry("Enter", () =>
                {
                    _audioManager.PlaySound3D(_sfxCharging,
                       transform.position, false);
                    _direction = _player.transform.position -
                        transform.position;
                    _direction.y = 0f;

                    float angle = -Mathf.Atan2(_direction.z, _direction.x) *
                        Mathf.Rad2Deg;

                    _cacodaemonParticles.transform.rotation =
                        Quaternion.Euler(0, angle, 0);
                    _cacodaemonParticles.Play();

                    _rigidbody.AddForce(_cacodaemonStatsContainer.
                        GetStat("MoveSpeed").Value * 3f * _direction.
                        normalized, ForceMode.Impulse);
                })
            ),

            new GenericState("GoingToCharge",
                new ActionEntry("Enter", () =>
                {
                    _audioManager.PlaySound3D(_sfxGoingToCharge,
                        transform.position, false);
                })
            ),

            new GenericState("Cooldown"),

            // Transitions

            new AllToOneTransition("Death", () =>
            {
                return _cacodaemonStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

            // Idle > Walk
            new RandomTimedTransition("Idle", "Walk", 1f, 2f),

            // Walk > Idle
            new FixedTimedTransition("Walk", "Idle", 0.7f),

            // Idle > Going to charge
            new GenericTransition("Idle", "GoingToCharge", () =>
            {
                return _distance < 1.0f;
            }),

            // Idle > Going to charge
            new FixedTimedTransition("GoingToCharge", "Charge", 0.5f),

            // Charge > Cooldown
            new FixedTimedTransition("Charge", "Cooldown", 0.7f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 0.7f)
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
            _stateMachine.CurrentState.StateID == "Charge");
        _animator.SetBool("isGoingCharge",
            _stateMachine.CurrentState.StateID == "GoingToCharge");
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
                _cacodaemonStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage.AddModifier(
                Modifier.Multiply(_cacodaemonStatsContainer.
                GetStat("DamageMultiplier").Value, 3)), 
                knockbackForce);
        }
    }
}
