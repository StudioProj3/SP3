using UnityEngine;

// Player controller class for movement.
// TODO (Chris): We should probably separate movement and other mechanics,
// so a PlayerMovement script and maybe a PlayerInventoryController script.
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField]
    [Range(0f, 10f)]
    private float _movementSpeed = 5f;

    #endregion

    #region Private Fields

    private Rigidbody _rigidbody;

    #endregion

    #region Private Functions

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
        // FIXME (Chris): These negative axes feel wrong/hacky.
        // Maybe fix our axes to a certain way? (maybe positive x)
        _rigidbody.velocity = _movementSpeed * new Vector3(
            -Input.GetAxisRaw("Horizontal"),
            0, -Input.GetAxisRaw("Vertical")).normalized;
    }

    #endregion
}
