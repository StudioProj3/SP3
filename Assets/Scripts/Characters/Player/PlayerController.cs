using UnityEngine;

// Player controller class for movement.
// TODO (Chris): We should probably separate movement and other mechanics,
// so a PlayerMovement script and maybe a PlayerInventoryController script.
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // TODO (Chris): Add a state machine for the player animations.

    [HorizontalDivider]
    [Header("Basic Parameters")]

    [SerializeField]
    [Range(0f, 10f)]
    private float _movementSpeed = 5f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _movementSpeed * new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0, Input.GetAxisRaw("Vertical")).normalized;
    }
}
