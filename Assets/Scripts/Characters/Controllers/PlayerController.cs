using Unity.VisualScripting;
using UnityEngine;

// Player controller class for movement.
// TODO (Chris): We should probably separate movement and other mechanics,
// so a PlayerMovement script and maybe a PlayerInventoryController script.
[DisallowMultipleComponent]
public class PlayerController :
    CharacterControllerBase
{
    [HorizontalDivider]
    [Header("Basic Parameters")]

    [SerializeField]
    [Range(0f, 10f)]
    private float _movementSpeed = 5f;

    [HorizontalDivider]
    [Header("Character Data")]

    [SerializeField]
    private PlayerData _playerData;
    
    [SerializeField]
    [Range(0f, 10f)]
    private float _rollForce = 5f;

    private float _horizontalInput;
    private float _verticalInput;
    private bool _rollKeyDown;

    protected override void Start()
    {
        base.Start();

        SetupStateMachine();
    }

    protected override void SetupStateMachine()
    {
        base.SetupStateMachine();

        _stateMachine.AddChilds
        (
            new GenericState("Walk",
                new ActionEntry("FixedUpdate", () =>
                {
                    _rigidbody.velocity = _movementSpeed * new Vector3(
                        _horizontalInput, 0, _verticalInput).normalized;
                })
            ),

            // NOTE (Brandon): Bug where transition to roll doesn't register when input is pressed
            // Only happens when direction is switched right before input
            new GenericState("Roll",
                new ActionEntry("Enter", () =>
                {
                    _animator.SetBool("facingFront", _verticalInput == -1);
                    _animator.SetBool("facingSide", _horizontalInput != 0);
                    Vector3 direction = new(_horizontalInput, 0, _verticalInput);
                    _rigidbody.AddForce(direction.normalized * _rollForce, ForceMode.Impulse);
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
            new TimedTransition("Roll", "Idle", 0.7f),

            // Walk or Idle > Roll
            new EagerGenericTransition("Walk", "Roll", () =>
            {
                return _rollKeyDown;
            }),

            new EagerGenericTransition("Idle", "Roll", () =>
            {
                return _rollKeyDown;
            })
        );
        
        _stateMachine.SetStartState("Idle");

        // Start state machine execution
        _stateMachine.Enter();
    }

    private void Update()
    {
        UpdateInputs();
        _animator.SetBool("isRunning", _stateMachine.CurrentState.StateID == "Walk");
        _animator.SetBool("isRolling", _stateMachine.CurrentState.StateID == "Roll");

        if (_horizontalInput != 0)
        {
            _spriteRenderer.flipX = _horizontalInput < 0;
        }
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }

    private void UpdateInputs()
    {
        _rollKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }
}
