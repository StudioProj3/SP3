using System.Collections.Generic;

using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController :
    CharacterControllerBase, IEffectable
{
    [HorizontalDivider]
    [Header("Character Data")]

    private float _horizontalInput;
    private float _verticalInput;

    private Vector3 _previousDirection = new(1, 0, 0);
    private bool _rollKeyPressed;

    public override void TakeDamage(Damage damage, Vector3 knockback)
    {
        _rigidbody.AddForce(knockback, ForceMode.Impulse);
        damage.OnApply(this);

        Color originalColor = _spriteRenderer.color;

        _spriteRenderer.color = new Color(1, 0, 0,0.5f);
        _ = Delay.Execute(() =>
        {
            _spriteRenderer.color = originalColor;
        }, 0.15f);

        if (Data.CharacterStats.GetStat("Health").Value <= 0)
        {
            GameManager.Instance.CurrentState = GameState.Lose;
        }
    }

    protected override void Start()
    {
        base.Start();
        EntityStats = Data.CharacterStats;
        UIHoverPanel.OnItemUse += ConsumeFromInventory;
        UIHoverPanel.OnItemDrop += DropFromInventory;
        UIPlayerRespawn.BeginPlayerRespawn += RespawnPlayer;
        
        SetupStateMachine();
    }

    protected void OnDestroy()
    {
        UIPlayerRespawn.BeginPlayerRespawn -= RespawnPlayer;
        UIHoverPanel.OnItemUse -= ConsumeFromInventory;
        UIHoverPanel.OnItemDrop -= DropFromInventory;
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds
        (
            new GenericState("Walk",
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = Data.CharacterStats.
                        GetStat("MoveSpeed").Value * new Vector3(
                        _horizontalInput, 0, _verticalInput).normalized;
                })
            ),

            new GenericState("Roll",
                new ActionEntry("Enter", () =>
                {
                    _animator.SetBool("facingFront",
                        _previousDirection.z == -1);
                    _animator.SetBool("facingSide",
                        _previousDirection.x != 0);

                    Vector3 direction = _previousDirection;

                    _rigidbody.AddForce(
                        Data.CharacterStats.GetStat("MoveSpeed").Value *
                        2 * direction.normalized,
                        ForceMode.Impulse);
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

    protected override void Awake()
    {
        base.Awake();
    }

    private void HandleStatusEffects()
    {
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
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Play)
        {
            return;
        }

        UpdateInputs();

        _animator.SetBool("isRunning", 
            _stateMachine.CurrentState.StateID == "Walk");
        _animator.SetBool("isRolling", 
            _stateMachine.CurrentState.StateID == "Roll");

        HandleStatusEffects();

        if (_horizontalInput != 0)
        {
            transform.rotation = Quaternion.Euler(0f,
                _horizontalInput < 0f ? 180f : 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Data.HandInventory.Print();
        }

        // Temporary bandaid solution
        // Ideally only check when damage is taken
        if (Data.CharacterStats.GetStat("Health").Value <= 0)
        {
            GameManager.Instance.CurrentState = GameState.Lose;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.CurrentState == GameState.Play)
        {
            _stateMachine.FixedUpdate();
        }
    }

    private void UpdateInputs()
    {
        if (!_rollKeyPressed)
        {
            if (_horizontalInput != 0 || _verticalInput != 0)
            {
                _previousDirection = new(_horizontalInput, 0, _verticalInput);
            }
            _rollKeyPressed = Input.GetKeyDown(KeyCode.LeftShift);
        }

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void RespawnPlayer()
    {
        GameManager.Instance.CurrentState = GameState.Play;
        LoadingManager.Instance.UnloadScene("UIDeathScreen");
        LoadingManager.Instance.LoadScene("SurfaceLayerScene");
    }

    private void ConsumeFromInventory(ItemBase item, int index, InventoryBase inventory)
    {
        if (item is IConsumable consumable)
        {
            consumable.ApplyConsumptionEffect(Data.CharacterStats, this);
            inventory.RemoveItemByIndex(index, 1);
        }
    }

    private void DropFromInventory(ItemBase item, int index, InventoryBase inventory)
    {
        GameObject spawner = GameObject.FindWithTag("ItemSpawner");
        if (spawner != null)
        {
            if (spawner.TryGetComponent(out ItemSpawner spawnerComponent))
            {
                Collectible collectible = 
                    spawnerComponent.SpawnObject(item, 1, transform.position);

                collectible.enabled = false;
                
                // After 0.5 seconds, enable collection again.
                this.DelayExecute(() => 
                {
                    collectible.enabled = true;
                }, 0.5f);
            }
        }
        inventory.RemoveItemByIndex(index, 1);
    }

}
