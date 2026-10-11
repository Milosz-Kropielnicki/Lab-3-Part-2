using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    // Wait time (min, max) between shots in seconds
    private float minFireTime = 2.0f;
    private float maxFireTime = 8.0f;

    // Distance from enemy for bullet to spawn
    private float spawnOffset = 1.0f;

    private float fireTimer;

    private int health = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetFireTimer();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;

        if (fireTimer <= 0.0f)
        {
            Fire();
            ResetFireTimer();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    void Fire()
    {
        // Spawn in front of enemy, toward player
        Vector3 spawnPosition = transform.position + Vector3.back * spawnOffset;

        // Creates bullet in scene root
        GameObject newBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);

        newBullet.GetComponent<BulletController>().Init(Vector3.back, false);
    }

    void ResetFireTimer()
    {
        fireTimer = Random.Range(minFireTime, maxFireTime);
    }
}
