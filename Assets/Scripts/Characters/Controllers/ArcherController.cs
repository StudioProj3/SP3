using System.Collections.Generic;

using UnityEngine;

public class ArcherController :
    CharacterControllerBase, IEffectable
{
    [SerializeField]
    private Stats _archerStats;

    [SerializeField]
    private StatusEffectBase _arrowStatusEffect;

    private GameObject _pooledArrows;
    private List<ArrowController> _pooledArrowList;

    private StatContainer _archerStatsContainer;

    private GameObject _player;
    private PlayerController _playerController;

    private List<StatusEffectBase> _statusEffects = new();

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    IStatContainer IEffectable.EntityStats => _archerStatsContainer;

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
        _pooledArrows = transform.GetChild(0).gameObject;
        _pooledArrowList = new List<ArrowController>();

        foreach (Transform child in _pooledArrows.transform)
        {
            _pooledArrowList.Add(child.GetComponent<ArrowController>());
        }

        _archerStatsContainer = _archerStats.GetInstancedStatContainer();
        _phyDamage = PhysicalDamage.Create(_archerStatsContainer.
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
                    _rigidbody.velocity = _archerStatsContainer.
                        GetStat("MoveSpeed").Value * _direction;
                })
            ),

            new GenericState("Shoot",
                new ActionEntry("Enter", () =>
                {
                    for (int i = 0; i < _pooledArrowList.Count; i++)
                    {
                        if (!(_pooledArrowList[i].gameObject.activeSelf))
                        {
                            _pooledArrowList[i].Init(_direction, _phyDamage
                                ,_arrowStatusEffect
                                ,_pooledArrows.transform);
                            _pooledArrowList[i].transform.position =
                                transform.position;
                            _pooledArrowList[i].transform.SetParent(null);

                            break;
                        }
                    }
                })
            ),

            new GenericState("Roll",
                new ActionEntry("Enter", () =>
                {
                    _direction = transform.position - _player.transform.position;
                    _direction.y = 0;

                    _rigidbody.AddForce( _archerStatsContainer.
                        GetStat("MoveSpeed").Value * 4f * _direction.normalized, 
                        ForceMode.Impulse);
                })
            ),

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
            new GenericTransition("Idle", "Roll", () =>
            {
                return _distance < 1.0f;
            }),

            // Idle > Going to shoot
            new GenericTransition("Idle", "GoingToShoot", () =>
            {
                return _distance < 3.0f;
            }),
           
            // Roll > Going to Shoot
            new FixedTimedTransition("Roll", "GoingToShoot", 0.5f),

            // FIXME (Aquila): Sometimes when archer shoots,
            // it plays the shoot animation but does not
            // shoot an arrow. Likely due to transition
            // timings between states

            //  Going to Shoot > Shoot
            new FixedTimedTransition("GoingToShoot", "Shoot", 0.6f),

            // Shoot > Cooldown
            new FixedTimedTransition("Shoot", "Cooldown", 0.2f),

            // Cooldown > Idle
            new FixedTimedTransition("Cooldown", "Idle", 0.2f)
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
        _animator.SetBool("isRolling",
         _stateMachine.CurrentState.StateID == "Roll");


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

        if (_archerStatsContainer.
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
                _archerStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage, knockbackForce);
        }
    }
}
