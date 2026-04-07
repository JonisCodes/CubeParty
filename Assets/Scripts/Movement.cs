using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    
    private Vector2 _moveInput;

    public Vector2 MoveInput
    {
        get => _moveInput;
        set => _moveInput = value;
    }

    [SerializeField] private Rigidbody rb;

    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private float acceleration = 5f;

    private void Move()
    {
        var camForward = cameraTransform.forward;
        var camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();
        
        var targetVelocity = (camForward * _moveInput.y + camRight * _moveInput.x) * movementSpeed;
        targetVelocity.y = rb.linearVelocity.y;

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * acceleration);
    }

    private void FixedUpdate()
    {
        Move();
    }
}