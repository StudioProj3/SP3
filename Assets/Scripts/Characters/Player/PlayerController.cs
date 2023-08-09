using UnityEngine;

// Player controller class for movement.
// TODO (Chris): We should probably separate movement and other mechanics,
// so a PlayerMovement script and maybe a PlayerInventoryController script.
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // TODO (Chris): Add a state machine for the player animations.
    #region Serialized Fields

    [HorizontalDivider]
    [Header("Basic Parameters")]

    [SerializeField]
    [Range(0f, 10f)]
    private float _movementSpeed = 5f;

    #endregion

    #region Private Fields

    private FiniteStateMachine<PlayerController> _stateMachine;

    private Rigidbody _rigidbody;

    #endregion

    #region Private Functions

    private void Start()
    {
        _stateMachine = new FiniteStateMachine<PlayerController>(this);
        _rigidbody = GetComponent<Rigidbody>(); 

        _stateMachine.SetState(typeof(PlayerWalkState));
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _movementSpeed * new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0, Input.GetAxisRaw("Vertical")).normalized;
    }

    #endregion
}
