using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float movementX;

    private float speed = 14.0f;

    private int health = 3;

    // How far left/right of center the player can go
    private float xBound = 20.0f;

    [SerializeField] private GameObject bullet;

    private float spawnOffset = 1.0f;

    private float fireCooldown = 0.3f;
    private float nextFireTime;

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

    void OnAttack()
    {
        if (Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + fireCooldown;

        Vector3 spawnPosition = transform.position + Vector3.forward * spawnOffset;

        GameObject newBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);

        newBullet.GetComponent<BulletController>().Init(Vector3.forward, true);
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
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