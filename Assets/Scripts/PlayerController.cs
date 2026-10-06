using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float movementX;

    public float speed = 10.0f;

    // How far left/right of center the player can go
    public float xBound = 8.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
    }

    void FixedUpdate()
    {
        // Set the velocity directly so movement starts and stops instantly
        rb.linearVelocity = new Vector3(movementX * speed, 0.0f, 0.0f);

        // Keep the player on screen
        Vector3 position = rb.position;
        position.x = Mathf.Clamp(position.x, -xBound, xBound);
        rb.position = position;
    }
}