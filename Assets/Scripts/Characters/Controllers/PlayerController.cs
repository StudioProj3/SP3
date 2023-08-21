using UnityEngine;

[DisallowMultipleComponent]
public class PlayerController :
    CharacterControllerBase, IEffectable
{
    [HorizontalDivider]
    [Header("Character Data")]

    private float _horizontalInput;
    private float _verticalInput;
    private bool _rollKeyPressed;

    protected override void Start()
    {
        base.Start();
        EntityStats = Data.CharacterStats;
        
        SetupStateMachine();
        Data.Reset();
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
                        _verticalInput == -1);
                    _animator.SetBool("facingSide",
                        _horizontalInput != 0);

                    Vector3 direction =
                        new(_horizontalInput, 0, _verticalInput);

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
            transform.rotation = Quaternion.Euler(0f,
                _horizontalInput < 0f ? 180f : 0f, 0f);
        }

        // Temporary bandaid solution
        // Ideally only check when damage is taken
        if (Data.CharacterStats.GetStat("Health").Value <= 0)
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
        }
        
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
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

    
}
