using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class TaxiMovement : PlayerInput
{
    public float acceleration = 15f;
    public float maxSpeed = 20f;
    public float turnSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector2 moveInput = _move.ReadValue<Vector2>();

        if (rb.linearVelocity.magnitude < maxSpeed)
        {
            Vector3 forwardForce = transform.forward * moveInput.y * acceleration;
            rb.AddForce(forwardForce);
        }

        if (Mathf.Abs(moveInput.x) > 0.1f && rb.linearVelocity.magnitude > 0.1f)
        {
            float turn = moveInput.x * turnSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
        }
    }
}
