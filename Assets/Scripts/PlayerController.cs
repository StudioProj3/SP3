using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player controller class for movement.
// TODO - We should probably separate movement and other mechanics, so a 
// TODO - PlayerMovement script and maybe a PlayerInventoryController script.
[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    #region Serialized Fields


    [SerializeField]
    private float _movementSpeed = 0.15f;

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
        // TODO - These negative axises feel wrong/hacky. Maybe fix our axis to a certain way? (maybe positive x)
        _rigidbody.velocity = _movementSpeed * new Vector3(
            -Input.GetAxisRaw("Horizontal"),
            0, -Input.GetAxisRaw("Vertical")).normalized;
    }

    #endregion
}
