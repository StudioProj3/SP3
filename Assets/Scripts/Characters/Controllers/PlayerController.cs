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

    private float _horizontalInput;
    private float _verticalInput;

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

            new EagerGenericTransition("Idle", "Walk", () =>
            {
                return _horizontalInput != 0 || _verticalInput != 0;
            }),

            // Walk State
            new UnconditionalTransition("Walk", "Idle")
        );
        
        _stateMachine.SetStartState("Idle");

        // Start state machine execution
        _stateMachine.Enter();

    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
}
