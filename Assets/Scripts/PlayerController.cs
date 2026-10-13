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

    // --- Powerup state ---
    private float upgradeDuration = 10.0f;
    private bool isUpgraded;
    private Coroutine upgradeRoutine;

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

        if (isUpgraded)
        {
            // Triple shot: center + slight left/right angles
            FireBullet(Vector3.forward, 0.0f);
            FireBullet(Quaternion.Euler(0, -15, 0) * Vector3.forward, -0.5f);
            FireBullet(Quaternion.Euler(0, 15, 0) * Vector3.forward, 0.5f);
        }
        else
        {
            FireBullet(Vector3.forward, 0.0f);
        }
    }

    void FireBullet(Vector3 direction, float xOffset)
    {
        Vector3 spawnPosition = transform.position + Vector3.forward * spawnOffset;
        spawnPosition += Vector3.right * xOffset;

        GameObject newBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
        newBullet.GetComponent<BulletController>().Init(direction, true, 1);
    }

    public void UpgradeWeapon()
    {
        isUpgraded = true;

        // Refresh timer if already upgraded
        if (upgradeRoutine != null)
        {
            StopCoroutine(upgradeRoutine);
        }
        upgradeRoutine = StartCoroutine(UpgradeTimer());
    }

    IEnumerator UpgradeTimer()
    {
        yield return new WaitForSeconds(upgradeDuration);

        isUpgraded = false;
        upgradeRoutine = null;
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

        while (Time.time < endTime)
        {
            playerRenderer.enabled = !playerRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        playerRenderer.enabled = true;
        isInvulnerable = false;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(movementX * speed, 0.0f, 0.0f);

        Vector3 position = rb.position;
        position.x = Mathf.Clamp(position.x, -xBound, xBound);
        rb.position = position;
    }
}