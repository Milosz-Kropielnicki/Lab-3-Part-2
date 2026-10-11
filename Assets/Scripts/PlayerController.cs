using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 startPosition;
    private float movementX;
    private float speed = 14.0f;

    private Renderer playerRenderer;

    private float invulnerableTime = 2.0f;
    private bool isInvulnerable;

    private float flashInterval = 0.1f;

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

        playerRenderer = GetComponentInChildren<Renderer>();
        startPosition = transform.position;
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
        if (isInvulnerable)
        {
            return;
        }

        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        rb.position = startPosition;
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;

        StartCoroutine(Invulnerability());
    }

    IEnumerator Invulnerability()
    {
        isInvulnerable = true;

        float endTime = Time.time + invulnerableTime;

        // Mesh toggling
        while (Time.time < endTime)
        {
            playerRenderer.enabled = !playerRenderer.enabled;

            yield return new WaitForSeconds(flashInterval);
        }

        // Make player visible when done
        playerRenderer.enabled = true;
        isInvulnerable = false;
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