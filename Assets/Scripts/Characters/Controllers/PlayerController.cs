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

    protected override void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _movementSpeed * new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0, Input.GetAxisRaw("Vertical")).normalized;
    }
}
