using System.Collections.Generic;
using UnityEngine;

// Player controller class for movement
// TODO (Chris): We should probably separate movement and other mechanics,
// so a PlayerMovement script and maybe a PlayerInventoryController script
[DisallowMultipleComponent]
public class PlayerController :
    CharacterControllerBase, IEffectable
{

    [field: SerializeField]
    private GameObject CurrentWeaponSlot;

    [HorizontalDivider]
    [Header("Character Data")]

    [SerializeField]
    private PlayerData _playerData;

    [SerializeField]
    private Stats _playerStats;

    //For debug
    [SerializeField]
    private BowWeaponItem _weaponItemTest;

    private ItemBase _currentlyHolding;
    private Animator _weaponAnimator;
    private SpriteRenderer _weaponDisplay;
    private float _horizontalInput;
    private float _verticalInput;
    private bool _rollKeyPressed;
    private GameObject _pooledArrows;
    private List<ArrowController> _pooledArrowList;
    private List<StatusEffectBase> _statusEffects = new();

    public IStatContainer EntityStats => _playerStats;

    public void TakeDamage(Damage damage, Vector3 knockback)
    {
        _rigidbody.AddForce(knockback, ForceMode.Impulse);
        damage.OnApply(this);

        if (_playerStats.GetStat("Health").Value <= 0)
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }
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

        _weaponAnimator = CurrentWeaponSlot.GetComponent<Animator>();
        _weaponDisplay = CurrentWeaponSlot.GetComponent<SpriteRenderer>();

        _pooledArrows = transform.GetChild(1).gameObject;
        _pooledArrowList = new List<ArrowController>();

        foreach (Transform child in _pooledArrows.transform)
        {
            _pooledArrowList.Add(child.GetComponent<ArrowController>());
        }
        
        // For debug
        Equip(_weaponItemTest);

        SetupStateMachine();


        // TODO (Cheng Jun): This should be updated to try
        // and fetch the player's local save instead of performing
        // a reset once the save system is ready
        _playerData.Reset();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds
        (
            new GenericState("Walk",
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _playerStats.
                        GetStat("MoveSpeed").Value * new Vector3(
                        _horizontalInput, 0, _verticalInput).normalized;
                })
            ),

            new GenericState("Roll",
                new ActionEntry("Enter", () =>
                {
                    _animator.SetBool("facingFront",
                        _verticalInput == -1);
                    _animator.SetBool("facingSide",
                        _horizontalInput != 0);

                    Vector3 direction =
                        new(_horizontalInput, 0, _verticalInput);

                    _rigidbody.AddForce(
                        _playerStats.GetStat("MoveSpeed").Value *
                        2 * direction.normalized,
                        ForceMode.Impulse
                        );
                }),

                new ActionEntry("Exit", () =>
                {
                    _rollKeyPressed = false;
                })
            ),

            // Transitions
            // Idle > Walk
            new EagerGenericTransition("Idle", "Walk", () =>
            {
                return _horizontalInput != 0 || _verticalInput != 0;
            }),

            // Walk > Idle
            new GenericTransition("Walk", "Idle", () =>
            {
                return _horizontalInput == 0 && _verticalInput == 0;
            }),

            // Roll > Idle
            new FixedTimedTransition("Roll", "Idle", 0.7f),

            // Walk or Idle > Roll
            new EagerGenericTransition("Walk", "Roll", () =>
            {
                return _rollKeyPressed;
            }),

            new EagerGenericTransition("Idle", "Roll", () =>
            {
                return _rollKeyPressed;
            })
        );
        
        _stateMachine.SetStartState("Idle");

        // Start state machine execution
        _stateMachine.Enter();
    }

    private void Awake()
    {
        WeaponDamage.OnWeaponHit += DealDamage;
    }

    private void Update()
    {
        UpdateInputs();

        _animator.SetBool("isRunning", 
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

        if (_horizontalInput != 0)
        {
            transform.localScale = new(_horizontalInput, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (_currentlyHolding is ISwordWeapon swordWeapon)
            {
                _weaponAnimator.Play(swordWeapon.AnimationName);
                _rigidbody.AddForce(2 * transform.localScale.x * transform.right , ForceMode.Impulse);
            }
            if (_currentlyHolding is IBowWeapon bowWeapon)
            {
                _weaponAnimator.Play(bowWeapon.AnimationName);
                _rigidbody.AddForce(2 * -transform.localScale.x * transform.right , ForceMode.Impulse);

                Vector3 shootDirection = transform.forward;
                for (int i = 0; i < _pooledArrowList.Count; i++)
                    {
                        if (!_pooledArrowList[i].gameObject.activeSelf)
                        {
                            bowWeapon.Shoot(_pooledArrowList[i], shootDirection, _pooledArrows.transform);
                        }
                    }
                }
            if (_currentlyHolding is IBeginUseHandler beginUseHandler)
            {
                beginUseHandler.OnUseEnter();
            }
        }

        Debug.Log(_playerStats.GetStat("Health").Value);
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();

        // Replace when item pickup is integrated with player
        Equip(_weaponItemTest);
    }

    private void UpdateInputs()
    {
        if (!_rollKeyPressed)
        {
            _rollKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        }
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void DealDamage(IEffectable effectable, Vector3 hitPos)
    {
        Vector3 knockbackForce = 
                (hitPos - transform.position).normalized *
                _playerStats.GetStat("Knockback").Value;
        effectable.TakeDamage(_weaponItemTest.WeaponDamageType, knockbackForce);

        if (_weaponItemTest.WeaponStatusEffect)
        {
            effectable.ApplyEffect(_weaponItemTest.WeaponStatusEffect.Clone());
        }
    }

    private void Equip(WeaponBase itemToEquip)
    {
        _weaponDisplay.sprite = itemToEquip.Sprite;
        _currentlyHolding = itemToEquip;
    }
}
