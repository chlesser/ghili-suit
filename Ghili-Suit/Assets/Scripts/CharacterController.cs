using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    private Vector2 _moveDirection;

    public InputActionReference move;

    private void Update()
    {
        _moveDirection = move.action.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
    }
    /* private void onEnable()
    {
        test.action.started += Test;
    }
    private void OnDisable()
    {
        test.action.started -= Test;
    }
    private void Test(InputAction.CallbackContext obj)
    {
        Debug.Log("Tested!");
    } */


}
