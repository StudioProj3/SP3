using System.Collections.Generic;

using UnityEngine;

public class ArcherController :
    EnemyControllerBase, IEffectable
{

    [SerializeField]
    private StatusEffectBase _arrowStatusEffect;

    private GameObject _pooledArrows;
    private List<ArrowController> _pooledArrowList;

    private StatContainer _archerStatsContainer;

    private GameObject _player;
    private PlayerController _playerController;

    private Vector3 _direction;
    private float _distance;
    private PhysicalDamage _phyDamage;

    protected override void Start()
    {
        base.Start();
        _pooledArrows = transform.GetChild(0).gameObject;
        _pooledArrowList = new List<ArrowController>();

        foreach (Transform child in _pooledArrows.transform)
        {
            _pooledArrowList.Add(child.GetComponent<ArrowController>());
        }

        _archerStatsContainer = Data.CharacterStats.
            GetInstancedStatContainer();

        EntityStats = _archerStatsContainer;
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
                            _pooledArrowList[i].Init(_direction, _phyDamage,
                                _arrowStatusEffect, _pooledArrows.transform);
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
                    _direction = transform.position -
                        _player.transform.position;
                    _direction.y = 0;

                    _rigidbody.AddForce( _archerStatsContainer.
                        GetStat("MoveSpeed").Value * 4f * _direction.normalized, 
                        ForceMode.Impulse);
                })
            ),

            new GenericState("GoingToShoot",
                new ActionEntry("Enter", () =>
                {
                    _direction = _player.transform.position -
                        transform.position;
                    _direction.y = 0;
                })
            ),

            new GenericState("Cooldown"),

            new GenericState("Death"),

            // Transitions

            new AllToOneTransition("Death", () =>
            {
                return _archerStatsContainer.
                    GetStat("Health").Value <= 0;
            }),

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
                _archerStatsContainer.GetStat("Knockback").Value;
            _playerController.TakeDamage(_phyDamage, knockbackForce);
        }
    }
}
